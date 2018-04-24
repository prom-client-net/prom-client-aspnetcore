# Prometheus.Client.AspNetCore

[![NuGet Badge](https://buildstats.info/nuget/Prometheus.Client.AspNetCore)](https://www.nuget.org/packages/Prometheus.Client.AspNetCore/)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](https://opensource.org/licenses/MIT)  


Extension for [Prometheus.Client](https://github.com/PrometheusClientNet/Prometheus.Client)


#### Installation:

     dotnet add package Prometheus.Client.AspNetCore

#### Quik start:

```csharp

public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
{
    app.UsePrometheusServer();
}

```
