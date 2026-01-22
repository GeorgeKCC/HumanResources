namespace Shared.DataProtection
{
    public static class UseServiceDataProtection
    {
        public static IServiceCollection AddServiceDataProtection(this IServiceCollection services)
        {
            services.AddDataProtection()
                 .PersistKeysToFileSystem(new DirectoryInfo("/dataprotection-keys"));

            return services;
        }
    }
}
