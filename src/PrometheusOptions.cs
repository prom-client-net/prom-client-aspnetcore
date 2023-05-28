using System;
using System.Text;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.AspNetCore
{
    /// <summary>
    ///     Options for Prometheus
    /// </summary>
    public class PrometheusOptions
    {
        public const string DefaultMapPath = "/metrics";

        /// <summary>
        ///     Url, default = "/metrics"
        /// </summary>
        public string MapPath { get; set; } = DefaultMapPath;

        /// <summary>
        ///     When specified only allow access to metrics on this port, otherwise return 404
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        ///     CollectorRegistry instance.
        /// </summary>
        public ICollectorRegistry CollectorRegistryInstance { get; set; }

        /// <summary>
        ///     Use default collectors
        /// </summary>
        public bool UseDefaultCollectors { get; set; } = true;

        /// <summary>
        ///     Charset of text response.
        /// </summary>
        public Encoding ResponseEncoding { get; set; }

        /// <summary>
        ///     Metric prefix for Default collectors
        /// </summary>
        public string MetricPrefixName { get; set; } = "";

        /// <summary>
        ///     Add legacy metrics to Default collectors
        /// </summary>
        ///  <remarks>
        ///     Some metrics renamed since v5, <c>AddLegacyMetrics</c> will add old and new name<br />
        ///     <para>
        ///       process_virtual_bytes -> process_virtual_memory_bytes<br />
        ///       process_private_bytes -> process_private_memory_bytes<br />
        ///       process_working_set -> process_working_set_bytes<br />
        ///       dotnet_totalmemory -> dotnet_total_memory_bytes
        ///     </para>
        /// </remarks>
        [Obsolete("'AddLegacyMetrics' will be removed in future versions")]
        public bool AddLegacyMetrics { get; set; }
    }
}
