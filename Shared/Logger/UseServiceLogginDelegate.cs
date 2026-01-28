using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Shared.Logger
{
    public static class UseServiceLogginDelegate
    {
        public static IServiceCollection AddServiceLogginDelegate(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<LoggingDelegatingHandler>();

            var seqUri = configuration.GetValue<string>("Seq:Uri")!;

            services.AddOpenTelemetry()
                      .WithTracing(tracing =>
                      {
                          tracing.AddSource("humanresourcesapi");
                          tracing.AddAspNetCoreInstrumentation(options =>
                          {
                              options.RecordException = true;
                          });
                          tracing.AddHttpClientInstrumentation(options =>
                          {
                              options.RecordException = true;
                          });
                          tracing.AddSqlClientInstrumentation(options =>
                          {
                              options.RecordException = true;
                          });
                          tracing.AddConsoleExporter();
                          tracing.AddOtlpExporter(opt =>
                          {
                              opt.Endpoint = new Uri($"{seqUri}/ingest/otlp/v1/traces");
                              opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                          });
                      });

            return services;
        }

        public static IApplicationBuilder UseAppSeg(this IApplicationBuilder app)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (ctx, http) =>
                {
                    ctx.Set("TraceId", Activity.Current?.TraceId.ToString());
                    ctx.Set("SpanId", Activity.Current?.SpanId.ToString());
                    ctx.Set("RequestPath", http.Request.Path);
                    ctx.Set("RequestMethod", http.Request.Method);
                };
            });

            return app;
        }


        public static ConfigureHostBuilder UseHostSerilog(this ConfigureHostBuilder host)
        {
            host.UseSerilog(SeriLogger.Configure);

            return host;
        }
    }
}
