using Elastic.CommonSchema;

namespace Shared.Securities
{
    public static class UseServiceAuthentication
    {
        /// <summary>
        /// Configuration in appsetting.json this sección code => "Token": {"SecretKey": "this section secret key","Issuer": "this section name issuer","Audience": ""this section name audience", "ExpireDay": "this section name Expire Day","TokenCookieName": "this section name cookie token name"}
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceAuthentication(this  IServiceCollection services, IConfiguration configuration)
        {
            TokenConfiguration token = GetConfigurationToken(configuration);
            string cookieName = token.TokenCookieName;

            services.AddAuthentication(options =>
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

            services.AddScoped<IGenerateToken, GenerateToken>();
            services.AddScoped<IPasswordHashWithSalt, PasswordHashWithSalt>();
            services.AddScoped<ITokensInsideCookie, TokensInsideCookie>();

            return services;
        }

        public static IApplicationBuilder UseAppUseAuthenticationAndAuthorization(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

        private static TokenConfiguration GetConfigurationToken(IConfiguration configuration)
        {
            var token = configuration.GetSection("Token").Get<TokenConfiguration>();
            return token ?? throw new TokenConfigurationException("Token Configuration AppSetting is missing.");
        }

    }
}
