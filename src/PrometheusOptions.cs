using System;
using System.Text;
using Prometheus.Client.Collectors;

namespace Prometheus.Client.AspNetCore;

/// <summary>
///     Options for Prometheus
/// </summary>
public class PrometheusOptions
{
    /// <summary>
    ///     Url, default = "/metrics"
    /// </summary>
    public string MapPath { get; set; } = Defaults.MapPath;

    /// <summary>
    ///     When specified only allow access to metrics on this port, otherwise return 404
    /// </summary>
    public int? Port { get; set; }

    /// <summary>
    ///     CollectorRegistry instance.
    /// </summary>
    public ICollectorRegistry CollectorRegistry { get; set; }

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
    public string MetricPrefixName { get; set; } = string.Empty;
}
