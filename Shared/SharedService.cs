using Shared.hub;

namespace Shared
{
    public static class SharedService
    {
        public static void BuilderSharedModule(WebApplicationBuilder webApplicationBuilder)
        {
            MapperConfiguration(webApplicationBuilder);
            webApplicationBuilder.Services.AddExceptionHandler<ExceptionHandler>();

        }

        public static IServiceCollection ServicesSharedModule(this IServiceCollection service, IConfiguration configuration)
        {
            ConfigDatabase(service, configuration);
            ConfigHybridCache(service, configuration);
            ConfigCors(service, configuration);
            ConfigAuthentication(service, configuration);
            ConfigHealthChecks(service, configuration);
            ConfigRateLimit(service);
            ConfigServices(service);
            ConfigSignalR(service);
            return service;
        }

        private static void ConfigSignalR(IServiceCollection service)
        {
            service.AddSignalR();
        }

        private static void ConfigServices(IServiceCollection service)
        {
            service.AddTransient<LoggingDelegatingHandler>();
            service.AddScoped<IGenerateToken, GenerateToken>();
            service.AddScoped<IPasswordHashWithSalt, PasswordHashWithSalt>();
            service.AddScoped<ITokensInsideCookie, TokensInsideCookie>();
        }

        private static void ConfigDatabase(IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContextPool<DatabaseContext>(x =>
            {
                x.UseSqlServer(configuration.GetConnectionString("Database"),
                               z => z.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            });
        }

        private static void ConfigHybridCache(IServiceCollection service, IConfiguration configuration)
        {
            service.AddMemoryCache();

            service.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });

            service.AddHybridCache(options =>
            {
                options.DefaultEntryOptions = new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(5),
                    LocalCacheExpiration = TimeSpan.FromMinutes(1)
                };
            });
        }

        private static void ConfigHealthChecks(IServiceCollection service, IConfiguration configuration)
        {
            service.AddHealthChecks()
                    .AddDbContextCheck<DatabaseContext>();

            service.AddHealthChecks()
                    .AddRedis(
                        configuration.GetConnectionString("Redis")!,
                        "Redis Health",
                        HealthStatus.Degraded);
        }

        private static void ConfigRateLimit(IServiceCollection service)
        {
            service.AddRateLimiter(options =>
            {
                options.AddPolicy("ip-policy", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: key => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        }));
            });
        }

        private static void ConfigAuthentication(IServiceCollection service, IConfiguration configuration)
        {
            TokenConfiguration token = GetConfigurationToken(configuration);
            string cookieName = token.TokenCookieName;

            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue(cookieName, out var token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        private static void ConfigCors(IServiceCollection service, IConfiguration configuration)
        {
            var urlCors = configuration.GetSection("UrlCors").Get<string[]>()!;
            service.AddCors(x =>
            {
                x.AddPolicy("CorsPolicy", z =>
                {
                    z.WithOrigins(urlCors);
                    z.AllowAnyHeader();
                    z.AllowAnyMethod();
                    z.AllowCredentials();
                });
            });
        }

        public static IApplicationBuilder ApplicationSharedModule(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(x => { });
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.Use(async (context, next) =>
            {
                var correlationId = context.Request.Headers["X-Correlation-Id"].FirstOrDefault()
                                    ?? Guid.NewGuid().ToString("N");
                context.Response.Headers["X-Correlation-Id"] = correlationId;
                using (Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId))
                {
                    await next();
                }
            });
            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(entry => new
                        {
                            name = entry.Key,
                            status = entry.Value.Status.ToString(),
                            exception = entry.Value.Exception?.Message,
                            duration = entry.Value.Duration.ToString()
                        })
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            });
            app.UseRateLimiter();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/hub/notifications")
                         .RequireAuthorization();
            });

            return app;
        }

        static void MapperConfiguration(WebApplicationBuilder builder)
        {
            builder.Services.Configure<TokenConfiguration>(builder.Configuration.GetSection("Token"));
            builder.Host.UseSerilog(SeriLogger.Configure);
        }

        static TokenConfiguration GetConfigurationToken(IConfiguration configuration)
        {
            var token = configuration.GetSection("Token").Get<TokenConfiguration>();

            return token ?? throw new TokenConfigurationException("Token Configuration AppSetting is missing.");
        }
    }
}
