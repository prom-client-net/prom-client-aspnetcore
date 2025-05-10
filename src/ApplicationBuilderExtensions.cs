using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.AspNetCore;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/> to configure Prometheus metrics middleware.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds Prometheus metrics middleware to the application pipeline.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> to configure.</param>
    /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
    public static IApplicationBuilder UsePrometheusServer(this IApplicationBuilder app)
    {
        return UsePrometheusServer(app, null);
    }

    /// <summary>
    /// Adds Prometheus metrics middleware to the application pipeline with custom options.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> to configure.</param>
    /// <param name="setupOptions">An <see cref="Action{PrometheusOptions}"/> to configure options.</param>
    /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="app"/> is <c>null</c>.</exception>
    public static IApplicationBuilder UsePrometheusServer(this IApplicationBuilder app, Action<PrometheusOptions> setupOptions)
    {
        ArgumentNullException.ThrowIfNull(app);

        var options = new PrometheusOptions
        {
            CollectorRegistry = (ICollectorRegistry)app.ApplicationServices.GetService(typeof(ICollectorRegistry)) ?? Metrics.DefaultCollectorRegistry
        };

        setupOptions?.Invoke(options);

        if (!options.MapPath.StartsWith('/'))
            options.MapPath = "/" + options.MapPath;

        if (options.UseDefaultCollectors)
        {
#pragma warning disable CS0618
            if (options.AddLegacyMetrics)
                options.CollectorRegistry.UseDefaultCollectors(options.MetricPrefixName, options.AddLegacyMetrics);
            else
                options.CollectorRegistry.UseDefaultCollectors(options.MetricPrefixName);
#pragma warning restore CS0618
        }

        var contentType = options.ResponseEncoding != null
            ? $"{Defaults.ContentType}; charset={options.ResponseEncoding.BodyName}"
            : Defaults.ContentType;

        return options.Port == null
            ? app.Map(options.MapPath, AddMetricsHandler)
            : app.Map(options.MapPath, cfg => cfg
                .MapWhen(ctx => ctx.Connection.LocalPort == options.Port, AddMetricsHandler));

        void AddMetricsHandler(IApplicationBuilder ab)
        {
            ab.Run(async context =>
            {
                var response = context.Response;
                response.ContentType = contentType;

                await using var outputStream = response.Body;
                await ScrapeHandler.ProcessAsync(options.CollectorRegistry, outputStream);
            });
        }
    }
}
