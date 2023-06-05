using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

#if OLDNETCORE
using Microsoft.AspNetCore.Builder.Internal;
#endif

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Prometheus.Client.Collectors;
using Prometheus.Client.Collectors.DotNetStats;
using Prometheus.Client.Collectors.ProcessStats;
using Xunit;

namespace Prometheus.Client.AspNetCore.Tests;

public class ApplicationBuilderExtensionsTests
{
    private readonly ICollectorRegistry _registry;
    private readonly IApplicationBuilder _app;
    private readonly HttpContext _ctx;

    public ApplicationBuilderExtensionsTests()
    {
        _registry = new CollectorRegistry();

        var services = new ServiceCollection();
        services.AddSingleton(_registry);
        _app = new ApplicationBuilder(services.BuildServiceProvider());

        _ctx = new DefaultHttpContext();
        _ctx.Request.Path = Defaults.MapPath;
    }

    [Fact]
    public void WhenApplicationBuilderIsNull_Throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ((ApplicationBuilder)null).UsePrometheusServer());
    }

    [Fact]
    public void DefaultPath_Return_200()
    {
        _app.UsePrometheusServer();
        _app.Build().Invoke(_ctx);

        Assert.Equal(200, _ctx.Response.StatusCode);
    }

    [Fact]
    public void UseDefaultCollectors_True_Register_DefaultCollectors()
    {
        _app.UsePrometheusServer();

        _registry.TryGet(nameof(ProcessCollector), out var processCollector);
        Assert.NotNull(processCollector);

        _registry.TryGet(nameof(GCTotalMemoryCollector), out var gcTotalMemoryCollector);
        Assert.NotNull(gcTotalMemoryCollector);

        _registry.TryGet(nameof(GCCollectionCountCollector), out var gcCollectionCountCollector);
        Assert.NotNull(gcCollectionCountCollector);
    }

    [Fact]
    public void UseDefaultCollectors_False_NotRegister_DefaultCollectors()
    {
        _app.UsePrometheusServer(q => q.UseDefaultCollectors = false);

        _registry.TryGet(nameof(ProcessCollector), out var processCollector);
        Assert.Null(processCollector);

        _registry.TryGet(nameof(GCTotalMemoryCollector), out var gcTotalMemoryCollector);
        Assert.Null(gcTotalMemoryCollector);

        _registry.TryGet(nameof(GCCollectionCountCollector), out var gcCollectionCountCollector);
        Assert.Null(gcCollectionCountCollector);
    }

    [Fact]
    public void AddLegacyMetrics_True_Contains_LegacyMetrics()
    {
        _app.UsePrometheusServer(q => q.AddLegacyMetrics = true);

        _registry.TryGet(nameof(ProcessCollector), out var processCollector);
        Assert.Contains("process_virtual_bytes", processCollector.MetricNames);
        Assert.Contains("process_private_bytes", processCollector.MetricNames);
        Assert.Contains("process_working_set", processCollector.MetricNames);

        _registry.TryGet(nameof(GCTotalMemoryCollector), out var gcTotalMemoryCollector);
        Assert.Contains("dotnet_totalmemory", gcTotalMemoryCollector.MetricNames);
    }

    [Fact]
    public void AddLegacyMetrics_False_DoesNotContain_LegacyMetrics()
    {
        _app.UsePrometheusServer();

        _registry.TryGet(nameof(ProcessCollector), out var processCollector);
        Assert.DoesNotContain("process_virtual_bytes", processCollector.MetricNames);
        Assert.DoesNotContain("process_private_bytes", processCollector.MetricNames);
        Assert.DoesNotContain("process_working_set", processCollector.MetricNames);

        _registry.TryGet(nameof(GCTotalMemoryCollector), out var gcTotalMemoryCollector);
        Assert.DoesNotContain("dotnet_totalmemory", gcTotalMemoryCollector.MetricNames);
    }

    [Theory]
    [InlineData("/path")]
    [InlineData("/test")]
    [InlineData("/test1")]
    public void CustomPath_Return_200(string path)
    {
        _app.UsePrometheusServer(q => { q.MapPath = path; });

        _ctx.Request.Path = $"{path}";
        _app.Build().Invoke(_ctx);

        Assert.Equal(200, _ctx.Response.StatusCode);
    }

    [Theory]
    [InlineData("path")]
    [InlineData("test")]
    [InlineData("test1")]
    public void CustomPath_Prepend_Slash_Return_200(string path)
    {
        _app.UsePrometheusServer(q => { q.MapPath = path; });

        _ctx.Request.Path = $"/{path}";
        _app.Build().Invoke(_ctx);

        Assert.Equal(200, _ctx.Response.StatusCode);
    }

    [Theory]
    [InlineData("/wrong")]
    [InlineData("/wr1")]
    [InlineData("/test")]
    public void WrongPath_Return_404(string path)
    {
        _app.UsePrometheusServer();

        _ctx.Request.Path = path;
        _app.Build().Invoke(_ctx);

        Assert.Equal(404, _ctx.Response.StatusCode);
    }

    [Fact]
    public void Default_ContentType()
    {
        _app.UsePrometheusServer();

        _app.Build().Invoke(_ctx);

        Assert.Equal(Defaults.ContentType, _ctx.Response.ContentType);
    }

    [Theory]
    [InlineData(1234)]
    [InlineData(8080)]
    [InlineData(5050)]
    public void CustomPort_Return_200(int port)
    {
        _app.UsePrometheusServer(q => { q.Port = port; });

        _ctx.Connection.LocalPort = port;
        _app.Build().Invoke(_ctx);

        Assert.Equal(200, _ctx.Response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetEncodings))]
    public void CustomResponseEncoding_Return_ContentType_With_Encoding(Encoding encoding)
    {
        _app.UsePrometheusServer(q => { q.ResponseEncoding = encoding; });

        _app.Build().Invoke(_ctx);

        Assert.Equal($"{Defaults.ContentType}; charset={encoding.BodyName}", _ctx.Response.ContentType);
    }

    public static IEnumerable<object[]> GetEncodings()
    {
        yield return new object[] { Encoding.UTF8 };
        yield return new object[] { Encoding.Unicode };
        yield return new object[] { Encoding.ASCII };
        yield return new object[] { Encoding.UTF32 };
    }
}
