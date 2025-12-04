using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using Shared.RedLock;
using StackExchange.Redis;

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
            ConfigHttpContextAccessor(service);
            ConfigDatabase(service, configuration);
            ConfigHybridCache(service, configuration);
            ConfigCors(service, configuration);
            ConfigAddAntiforgery(service);
            ConfigAuthentication(service, configuration);
            ConfigHealthChecks(service, configuration);
            ConfigRateLimit(service);
            ConfigServices(service);
            ConfigSignalR(service);
            ConfigDataProtection(service);
            return service;
        }

        private static void ConfigHttpContextAccessor(IServiceCollection service)
        {
            service.AddHttpContextAccessor();
        }

        public static IApplicationBuilder ApplicationSharedModule(this IApplicationBuilder app)
        {
            app.UseCors("CorsPolicy");
            app.UseAntiforgery();
            app.UseMiddleware<CorrelationMiddleware>();
            app.UseMiddleware<RedLockMiddleware>();
            app.UseExceptionHandler(x => { });
            app.UseAuthentication();
            app.UseAuthorization();
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

            return app;
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

            service.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis") 
                                          ?? throw new Ex("Not found connection redis")));


            //service.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = configuration.GetConnectionString("Redis");
            //});

            service.AddHybridCache(options =>
            {
                options.DefaultEntryOptions = new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(5),
                    LocalCacheExpiration = TimeSpan.FromMinutes(1)
                };
            });

            service.AddSingleton<RedLockFactory>(sp =>
            {
                var mux = sp.GetRequiredService<IConnectionMultiplexer>();
                if (mux == null || !mux.IsConnected)
                    throw new InvalidOperationException("Redis connection is not available for RedLockFactory.");

                return RedLockFactory.Create(
                [
                    new RedLockMultiplexer(mux)
                ]);
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

        static void MapperConfiguration(WebApplicationBuilder builder)
        {
            builder.Services.Configure<TokenConfiguration>(builder.Configuration.GetSection("Token"));
            builder.Host.UseSerilog(SeriLogger.Configure);
            builder.Host.UseDefaultServiceProvider(options =>
            {
                options.ValidateOnBuild = true;
                options.ValidateScopes = true;
            });
        }

        static TokenConfiguration GetConfigurationToken(IConfiguration configuration)
        {
            var token = configuration.GetSection("Token").Get<TokenConfiguration>();
            return token ?? throw new TokenConfigurationException("Token Configuration AppSetting is missing.");
        }

        private static void ConfigAddAntiforgery(IServiceCollection service)
        {
            service.AddAntiforgery(options =>
            {
                options.Cookie.Name = CSRF_Constant.KEY;
                options.Cookie.HttpOnly = false;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.HeaderName = CSRF_Constant.KEY;
            });
        }

        private static void ConfigDataProtection(IServiceCollection service)
        {
            service.AddDataProtection()
                   .PersistKeysToFileSystem(new DirectoryInfo("/dataprotection-keys"));
        }
    }
}