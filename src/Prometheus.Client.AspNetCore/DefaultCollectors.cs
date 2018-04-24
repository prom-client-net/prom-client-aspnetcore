using System.Collections.Generic;
using Prometheus.Client.Collectors;
using System.Runtime.InteropServices;

namespace Prometheus.Client.AspNetCore
{
    /// <summary>
    ///     All default Collector
    /// </summary>
    public static class DefaultCollectors
    {
        /// <summary>
        ///     Get default Collector
        /// </summary>
        public static IEnumerable<IOnDemandCollector> Get(MetricFactory metricFactory)
        {
            yield return new DotNetStatsCollector(metricFactory);
            
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (isWindows)
                yield return new WindowsDotNetStatsCollector(metricFactory);

        }
    }
}