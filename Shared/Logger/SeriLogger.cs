namespace Shared.Logger
{
    public static class SeriLogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
           (context, configuration) =>
           {
               var seqUri = context.Configuration.GetValue<string>("Seq:Uri")!;

               configuration
                   .ReadFrom.Configuration(context.Configuration)
                   .Enrich.FromLogContext()
                   .Enrich.WithCorrelationId()
                   .Enrich.WithProperty("Service", "humanresourcesapi")
                   .Enrich.WithActivityId()
                   .Enrich.WithTraceIdentifier()
                   .WriteTo.Console()
                   .WriteTo.Seq(seqUri, bufferBaseFilename: "/tmp/seq-buffer");
           };
    }
}

