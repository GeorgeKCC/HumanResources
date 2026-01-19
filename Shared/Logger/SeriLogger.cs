namespace Shared.Logger
{
    public static class SeriLogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
           (context, configuration) =>
           {
               var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri")!;
               var indexFormat = $"applogs-{context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}";

               configuration
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                    .WriteTo.Debug()
                    .WriteTo.Console()
                    .WriteTo.Elasticsearch([new Uri(elasticUri)], opts =>
                    {
                        opts.DataStream = new DataStreamName(indexFormat);
                    }, transport =>
                    {
                        transport.Authentication(new BasicAuthentication("kibana_system", "george25"));
                    })
                    .ReadFrom.Configuration(context.Configuration);
           };
    }
}

