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
using Xunit;

namespace Prometheus.Client.AspNetCore.Tests;

public class ApplicationBuilderExtensionsTests
{
    private readonly ICollectorRegistry _registry;
    private readonly IApplicationBuilder _app;
    private readonly HttpContext _ctx;

    public ApplicationBuilderExtensionsTests()
    {
        var services = new ServiceCollection();
        _app = new ApplicationBuilder(services.BuildServiceProvider());
        _registry = new CollectorRegistry();
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
        _app.UsePrometheusServer(q => q.CollectorRegistryInstance = _registry);
        _app.Build().Invoke(_ctx);

        Assert.Equal(200, _ctx.Response.StatusCode);
    }

    [Theory]
    [InlineData("/path")]
    [InlineData("/test")]
    [InlineData("/test1")]
    public void CustomPath_Return_200(string path)
    {
        _app.UsePrometheusServer(q =>
        {
            q.CollectorRegistryInstance = _registry;
            q.MapPath = path;
        });

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
        _app.UsePrometheusServer(q =>
        {
            q.CollectorRegistryInstance = _registry;
            q.MapPath = path;
        });

        _ctx.Request.Path = $"/{path}";
        _app.Build().Invoke(_ctx);

        Assert.Equal(200, _ctx.Response.StatusCode);
    }

    [Fact]
    public void WrongPath_Return_404()
    {
        _app.UsePrometheusServer(q => q.CollectorRegistryInstance = _registry);

        _ctx.Request.Path = "/wrong";
        _app.Build().Invoke(_ctx);

        Assert.Equal(404, _ctx.Response.StatusCode);
    }

    [Fact]
    public void Default_ContentType()
    {
        _app.UsePrometheusServer(q => q.CollectorRegistryInstance = _registry);

        _app.Build().Invoke(_ctx);

        Assert.Equal(Defaults.ContentType, _ctx.Response.ContentType);
    }

    [Theory]
    [InlineData(1234)]
    [InlineData(8080)]
    [InlineData(5050)]
    public void CustomPort_Return_200(int port)
    {
        _app.UsePrometheusServer(q =>
        {
            q.CollectorRegistryInstance = _registry;
            q.Port = port;
        });

        _ctx.Connection.LocalPort = port;
        _app.Build().Invoke(_ctx);

        Assert.Equal(200, _ctx.Response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetEncodings))]
    public void CustomResponseEncoding_Return_ContentType_With_Encoding(Encoding encoding)
    {
        _app.UsePrometheusServer(q =>
        {
            q.CollectorRegistryInstance = _registry;
            q.ResponseEncoding = encoding;
        });

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
