# Prometheus.Client.AspNetCore

[![NuGet](https://img.shields.io/nuget/v/Prometheus.Client.AspNetCore.svg)](https://www.nuget.org/packages/Prometheus.Client.AspNetCore)
[![CI](https://img.shields.io/github/workflow/status/prom-client-net/prom-client-aspnetcore/%F0%9F%92%BF%20CI%20Master?label=CI&logo=github)](https://github.com/prom-client-net/prom-client-aspnetcore/actions/workflows/master.yml)
[![CodeFactor](https://www.codefactor.io/repository/github/prom-client-net/prom-client-aspnetcore/badge)](https://www.codefactor.io/repository/github/prom-client-net/prom-client-aspnetcore)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT)

Extension for [Prometheus.Client](https://github.com/prom-client-net/prom-client)

## Installation

```sh
dotnet add package Prometheus.Client.AspNetCore
```

## Use

There are [Examples](https://github.com/prom-client-net/prom-examples/tree/master/Middleware)

```c#

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
{
    app.UsePrometheusServer();
}

```

```c#

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
{
    app.UsePrometheusServer(q =>
    {
        q.MapPath = "/metrics1";
    });
}

```

## Contribute

Contributions to the package are always welcome!

* Report any bugs or issues you find on the [issue tracker](https://github.com/PrometheusClientNet/Prometheus.Client.AspNetCore/issues).
* You can grab the source code at the package's [git repository](https://github.com/PrometheusClientNet/Prometheus.Client.AspNetCore).

## License

All contents of this package are licensed under the [MIT license](https://opensource.org/licenses/MIT).
