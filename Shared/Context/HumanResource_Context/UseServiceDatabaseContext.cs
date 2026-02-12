using Shared.Context.HumanResource_Context.Seed.HumanResourceSeed;

namespace Shared.Context.HumanResource_Context
{
    public static class UseServiceDatabaseContext
    {
        public static IServiceCollection AddServiceDatabaseHumanResourceContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<DatabaseHumanResourceContext>(x =>
            {
                x.UseSqlServer(configuration.GetConnectionString("Database"),
                               z => z.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            });

            services.AddHealthChecks()
                    .AddDbContextCheck<DatabaseHumanResourceContext>();

            return services;
        }

        public static IApplicationBuilder UseAppSeedDatabaseHumanResource(this WebApplication app)
        {
           SeedDatabaseHumanResource.InitDatabase(app.Services).GetAwaiter().GetResult();
            return app;
        }
    }
}
