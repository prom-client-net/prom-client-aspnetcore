using System;
using Prometheus.Client.Collectors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Prometheus.Client.Collectors.Abstractions;

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
            options.CollectorRegistryInstance
                = (ICollectorRegistry)app.ApplicationServices.GetService(typeof(ICollectorRegistry)) ?? Metrics.DefaultCollectorRegistry;

            setupOptions?.Invoke(options);

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (!options.MapPath.StartsWith("/"))
                throw new ArgumentException($"MapPath '{options.MapPath}' should start with '/'");

            if (options.UseDefaultCollectors)
                options.CollectorRegistryInstance.UseDefaultCollectors();

            void AddMetricsHandler(IApplicationBuilder coreapp)
            {
                coreapp.Run(async context =>
                {
                    var response = context.Response;
                    response.ContentType = "text/plain; version=0.0.4";

                    using var outputStream = response.Body;
                    await ScrapeHandler.ProcessAsync(options.CollectorRegistryInstance, outputStream);
                });
            }

            if (options.Port == null)
                return app.Map(options.MapPath, AddMetricsHandler);

            bool PortMatches(HttpContext context) => context.Connection.LocalPort == options.Port;
            return app.Map(options.MapPath, cfg => cfg.MapWhen(PortMatches, AddMetricsHandler));
        }
    }
}
