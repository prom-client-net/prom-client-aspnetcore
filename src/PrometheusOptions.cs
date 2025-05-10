using System.Text;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.AspNetCore;

/// <summary>
/// Configuration options for Prometheus metrics middleware.
/// </summary>
public class PrometheusOptions
{
    /// <summary>
    /// The endpoint path for metrics. Default is "/metrics".
    /// </summary>
    public string MapPath { get; set; } = Defaults.MapPath;

    /// <summary>
    /// Port restriction for metrics endpoint.
    /// </summary>
    public int? Port { get; set; }

    /// <summary>
    /// The <see cref="ICollectorRegistry"/> instance to use for metric collection.
    /// </summary>
    public ICollectorRegistry CollectorRegistry { get; set; }

    /// <summary>
    /// Whether to register default system metric collectors. Default is <c>true</c>.
    /// </summary>
    public bool UseDefaultCollectors { get; set; } = true;

    /// <summary>
    /// The text encoding for response content.
    /// </summary>
    public Encoding ResponseEncoding { get; set; }

    /// <summary>
    /// Metric name prefix for default collectors.
    /// </summary>
    public string MetricPrefixName { get; set; } = string.Empty;
}
