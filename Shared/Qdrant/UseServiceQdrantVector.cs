using Elastic.CommonSchema;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Qdrant
{
    public static class UseServiceQdrantVector
    {
        /// <summary>
        /// Configuration in appsetting.json this sección code => "Qdrant": { "Host": "this section name host string", "GrpcPort": this section port grpc type number int, "TimeoutSeconds": this section timeOut in second type number int }
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="Ex"></exception>
        public static IServiceCollection AddServiceQdrant(this IServiceCollection services, IConfiguration configuration)
        {
            var qdrant = configuration.GetSection("Qdrant").Get<QdrantConfiguration>() ?? throw new Ex("Not implemented configuration Qdrant");

            services.AddSingleton<QdrantClient>(sp =>
            {
                return new QdrantClient(
                    host: qdrant.Host,
                    port: qdrant.GrpcPort,
                    grpcTimeout: TimeSpan.FromSeconds(qdrant.TimeoutSeconds));
            });

            services.AddScoped<IQdrantRepository, ColaboratorQdrantRepository>();

            return services;
        }
    }
}
