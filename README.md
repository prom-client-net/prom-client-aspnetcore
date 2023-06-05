# Prometheus.Client.AspNetCore

[![ci](https://img.shields.io/github/actions/workflow/status/prom-client-net/prom-client-aspnetcore/ci.yml?branch=main&label=ci&logo=github&style=flat-square)](https://github.com/prom-client-net/prom-client-aspnetcore/actions/workflows/ci.yml)
[![nuget](https://img.shields.io/nuget/v/Prometheus.Client.AspNetCore?logo=nuget&style=flat-square)](https://www.nuget.org/packages/Prometheus.Client.AspNetCore)
[![nuget](https://img.shields.io/nuget/dt/Prometheus.Client.AspNetCore?logo=nuget&style=flat-square)](https://www.nuget.org/packages/Prometheus.Client.AspNetCore)
[![codecov](https://img.shields.io/codecov/c/github/prom-client-net/prom-client-aspnetcore?logo=codecov&style=flat-square)](https://app.codecov.io/gh/prom-client-net/prom-client-aspnetcore)
[![codefactor](https://img.shields.io/codefactor/grade/github/prom-client-net/prom-client-aspnetcore?logo=codefactor&style=flat-square)](https://www.codefactor.io/repository/github/prom-client-net/prom-client-aspnetcore)
[![license](https://img.shields.io/github/license/prom-client-net/prom-client-aspnetcore?style=flat-square)](https://github.com/prom-client-net/prom-client-aspnetcore/blob/main/LICENSE)

Extension for [Prometheus.Client](https://github.com/prom-client-net/prom-client)

## Installation

```sh
dotnet add package Prometheus.Client.AspNetCore
```

## Use

There are [Examples](https://github.com/prom-client-net/prom-examples)

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

* Report any bugs or issues you find on the [issue tracker](https://github.com/prom-client-net/prom-client-aspnetcore/issues).
* You can grab the source code at the package's [git repository](https://github.com/prom-client-net/prom-client-aspnetcore).

## License

All contents of this package are licensed under the [MIT license](https://opensource.org/licenses/MIT).
