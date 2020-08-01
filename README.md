# Prometheus.Client.AspNetCore

[![NuGet](https://img.shields.io/nuget/v/Prometheus.Client.AspNetCore.svg)](https://www.nuget.org/packages/Prometheus.Client.AspNetCore)
[![NuGet](https://img.shields.io/nuget/dt/Prometheus.Client.AspNetCore.svg)](https://www.nuget.org/packages/Prometheus.Client.AspNetCore)
[![CI](https://github.com/PrometheusClientNet/Prometheus.Client.AspNetCore/workflows/CI/badge.svg)](https://github.com/PrometheusClientNet/Prometheus.Client.AspNetCore/actions?query=workflow%3ACI)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT)



Extension for [Prometheus.Client](https://github.com/PrometheusClientNet/Prometheus.Client)


#### Installation:

     dotnet add package Prometheus.Client.AspNetCore

#### Quik start:

There are [Examples](https://github.com/PrometheusClientNet/Prometheus.Client.Examples/tree/master/Middleware/WebAspNetCore_2.0)

```csharp

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
{
    app.UsePrometheusServer();
}

```

```csharp

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
{
    app.UsePrometheusServer(q =>
    {
        q.MapPath = "/metrics1";
    });
}

```


