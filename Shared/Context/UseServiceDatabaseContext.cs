namespace Shared.Context
{
    public static class UseServiceDatabaseContext
    {
        public static IServiceCollection AddServiceDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<DatabaseContext>(x =>
            {
                x.UseSqlServer(configuration.GetConnectionString("Database"),
                               z => z.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            });

            services.AddHealthChecks()
                    .AddDbContextCheck<DatabaseContext>();

            return services;
        }
    }
}
