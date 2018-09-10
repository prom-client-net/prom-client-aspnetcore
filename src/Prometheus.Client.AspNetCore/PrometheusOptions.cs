using System.Collections.Generic;
using Prometheus.Client.Collectors;
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
        ///     CollectorRegistry intance
        /// </summary>
        public ICollectorRegistry CollectorRegistryInstance { get; set; } = CollectorRegistry.Instance;

        /// <summary>
        ///     IOnDemandCollectors
        /// </summary>
        public List<IOnDemandCollector> Collectors { get; set; } = new List<IOnDemandCollector>();
        
        /// <summary>
        ///     Use default collectors
        /// </summary>
        public bool UseDefaultCollectors { get; set; } = true;
    }
}