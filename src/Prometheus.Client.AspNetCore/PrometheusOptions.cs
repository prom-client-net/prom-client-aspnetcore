using Prometheus.Client.Collectors.Abstractions;

namespace Prometheus.Client.AspNetCore
{
    /// <summary>
    ///     Options for Prometheus
    /// </summary>
    public class PrometheusOptions
    {
        /// <summary>
        ///     Url, default = "/metrics"
        /// </summary>
        public string MapPath { get; set; } = "/metrics";

        /// <summary>
        ///     When specified only allow access to metrics on this port, otherwise return 404, default = null.
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        ///     CollectorRegistry intance
        /// </summary>
        public ICollectorRegistry CollectorRegistryInstance { get; set; } = Metrics.DefaultCollectorRegistry;

        /// <summary>
        ///     Use default collectors
        /// </summary>
        public bool UseDefaultCollectors { get; set; } = true;
    }
}
