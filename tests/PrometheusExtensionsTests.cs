using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

#if OLDNETCORE
using Microsoft.AspNetCore.Builder.Internal;
#endif

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Prometheus.Client.Collectors;
using Xunit;

namespace Prometheus.Client.AspNetCore.Tests;

public class PrometheusExtensionsTests
{
    private readonly ICollectorRegistry _registry;
    private readonly IApplicationBuilder _app;

    public PrometheusExtensionsTests()
    {
        var services = new ServiceCollection();
        _app = new ApplicationBuilder(services.BuildServiceProvider());
        _registry = new CollectorRegistry();
    }

    [Fact]
    public void UsePrometheusServer_DefaultUrl_Return_200()
    {
        _app.UsePrometheusServer(q => q.CollectorRegistryInstance = _registry);
        var reqDelegate = _app.Build();

        HttpContext ctx = new DefaultHttpContext();
        ctx.Request.Path = PrometheusOptions.DefaultMapPath;
        reqDelegate.Invoke(ctx);

        Assert.Equal(200, ctx.Response.StatusCode);
    }

    [Fact]
    public void UsePrometheusServer_Default_ContentType()
    {
        _app.UsePrometheusServer(q => q.CollectorRegistryInstance = _registry);
        var reqDelegate = _app.Build();

        HttpContext ctx = new DefaultHttpContext();
        ctx.Request.Path = PrometheusOptions.DefaultMapPath;
        reqDelegate.Invoke(ctx);

        Assert.Equal("text/plain; version=0.0.4", ctx.Response.ContentType);
    }

    [Theory]
    [MemberData(nameof(GetEncodings))]
    public void UsePrometheusServer_CustomResponseEncoding_Return_ContentType_With_Encoding(Encoding encoding)
    {
        _app.UsePrometheusServer(q =>
        {
            q.CollectorRegistryInstance = _registry;
            q.ResponseEncoding = encoding;
        });
        var reqDelegate = _app.Build();

        HttpContext ctx = new DefaultHttpContext();
        ctx.Request.Path = PrometheusOptions.DefaultMapPath;
        reqDelegate.Invoke(ctx);

        Assert.Equal($"text/plain; version=0.0.4; charset={encoding.BodyName}", ctx.Response.ContentType);
    }

    [Fact]
    public void UsePrometheusServer_WrongUrl_Return_404()
    {
        _app.UsePrometheusServer();
        var reqDelegate = _app.Build();

        HttpContext ctx = new DefaultHttpContext();
        ctx.Request.Path = "/wrong";
        reqDelegate.Invoke(ctx);

        Assert.Equal(404, ctx.Response.StatusCode);
    }

    public static IEnumerable<object[]> GetEncodings()
    {
        yield return new object[] { Encoding.UTF8 };
        yield return new object[] { Encoding.Unicode };
        yield return new object[] { Encoding.ASCII };
        yield return new object[] { Encoding.UTF7 };
    }
}
