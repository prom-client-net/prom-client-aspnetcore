using System;
using System.Threading.Tasks;
using Prometheus.Client.Collectors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Prometheus.Client.AspNetCore
{
    /// <summary>
    ///     PrometheusExtensions
    /// </summary>
    public static class PrometheusExtensions
    {
        /// <summary>
        ///     Add PrometheusServer request execution pipeline.
        /// </summary>
        public static IApplicationBuilder UsePrometheusServer(this IApplicationBuilder app)
        {
            return UsePrometheusServer(app, null);
        }

        /// <summary>
        ///     Add PrometheusServer request execution pipeline.
        /// </summary>
        public static IApplicationBuilder UsePrometheusServer(this IApplicationBuilder app, Action<PrometheusOptions> setupOptions)
        {
            var options = new PrometheusOptions();
            setupOptions?.Invoke(options);

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (!options.MapPath.StartsWith("/"))
                throw new ArgumentException($"MapPath '{options.MapPath}' should start with '/'");

            RegisterCollectors(options);

            void AddMetricsHandler(IApplicationBuilder coreapp)
            {
                coreapp.Run(async context =>
                {
                    var response = context.Response;
                    response.ContentType = "text/plain; version=0.0.4";

                    using (var outputStream = response.Body)
                    {
                        ScrapeHandler.Process(options.CollectorRegistryInstance, outputStream);
                    }

                    await Task.FromResult(0).ConfigureAwait(false);
                });
            }

            if (options.Port == null)
                return app.Map(options.MapPath, AddMetricsHandler);

            bool PortMatches(HttpContext context) => context.Connection.LocalPort == options.Port;
            return app.Map(options.MapPath, cfg => cfg.MapWhen(PortMatches, AddMetricsHandler));
        }

        private static void RegisterCollectors(PrometheusOptions options)
        {
            if (options.UseDefaultCollectors)
                options.CollectorRegistryInstance.UseDefaultCollectors();

            foreach (var collector in options.Collectors)
                options.CollectorRegistryInstance.Add("", collector); //todo: Add Name
        }
    }
}
