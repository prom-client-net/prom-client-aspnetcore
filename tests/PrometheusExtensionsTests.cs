using Microsoft.AspNetCore.Builder;

#if OLDNETCORE
using Microsoft.AspNetCore.Builder.Internal;
#endif

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Prometheus.Client.AspNetCore.Tests;

public class PrometheusExtensionsTests
{
    private readonly IApplicationBuilder _app;

    public PrometheusExtensionsTests()
    {
        var services = new ServiceCollection();
        _app = new ApplicationBuilder(services.BuildServiceProvider());
    }

    [Fact]
    public void UsePrometheusServer_CorrectUrl_Return_200()
    {
        _app.UsePrometheusServer();
        var reqDelegate = _app.Build();

        HttpContext ctx = new DefaultHttpContext();
        ctx.Request.Path = "/metrics";
        reqDelegate.Invoke(ctx);

        Assert.Equal(200, ctx.Response.StatusCode);
        Assert.Equal("text/plain; version=0.0.4", ctx.Response.ContentType);
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
}
