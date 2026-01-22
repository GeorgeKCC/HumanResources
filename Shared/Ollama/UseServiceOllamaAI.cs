namespace Shared.Ollama
{
    public static class UseServiceOllamaAI
    {
        /// <summary>
        /// Configuration in appsetting.json this sección code => "Ollama": { "OllamaBaseUrl": "this section add url connection type string" }
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="Ex"></exception>
        public static IServiceCollection AddServiceOllama(this IServiceCollection services, IConfiguration configuration)
        {
            var ollama = configuration.GetSection("Ollama").Get<OllamaConfiguration>() ?? throw new Ex("Not implemented configuration Ollama");

            services.AddHttpClient<IOllamaService, OllamaService>(client =>
            {
                client.BaseAddress = new Uri(ollama.OllamaBaseUrl);
            });

            return services;

        }
    }
}
