[bmac]: https://www.buymeacoffee.com/phnx47
[ko-fi]: https://ko-fi.com/phnx47
[patreon]: https://www.patreon.com/phnx47

# Prometheus.Client.AspNetCore

[![NuGet](https://img.shields.io/nuget/v/Prometheus.Client.AspNetCore.svg)](https://www.nuget.org/packages/Prometheus.Client.AspNetCore)
[![CI Master](https://github.com/PrometheusClientNet/Prometheus.Client.AspNetCore/actions/workflows/master.yml/badge.svg?branch=master)](https://github.com/PrometheusClientNet/Prometheus.Client.AspNetCore/actions/workflows/master.yml)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT)

Extension for [Prometheus.Client](https://github.com/PrometheusClientNet/Prometheus.Client)


#### Installation:
```sh
dotnet add package Prometheus.Client.AspNetCore
```
#### Quik start:

There are [Examples](https://github.com/PrometheusClientNet/Prometheus.Client.Examples/tree/master/Middleware/WebAspNetCore_2.0)

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

## Support

If you like what I'm accomplishing, feel free to buy me a coffee

[<img align="left" alt="phnx47 | Buy Me a Coffe" width="32px" src="https://raw.githubusercontent.com/phnx47/files/master/button-sponsors/bmac0.png" />][bmac]
[<img align="left" alt="phnx47 | Kofi" width="32px" src="https://raw.githubusercontent.com/phnx47/files/master/button-sponsors/kofi0.png" />][ko-fi]
[<img align="left" alt="phnx47 | Patreon" width="32px" src="https://raw.githubusercontent.com/phnx47/files/master/button-sponsors/patreon0.png" />][patreon]

&nbsp;

## License

All contents of this package are licensed under the [MIT license](https://opensource.org/licenses/MIT).

