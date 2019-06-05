﻿DNC Framework with .NETStandard 2.0
===

# Dnc简介
基于StandardLibrary的快速开发框架，提供了稳定强大、方便易用的事件处理机制和高性能高吞吐的CQRS组件。

# Dnc特性

* 稳定强大、方便易用的事件处理机制
* 高性能高吞吐的CQRS组件
* 稳定强大、方便易用的数据库仓储
* 按需安装加载的插件模式
* 按需安装加载的业务模块取用机制
* 为快速方便构建实践DDD提供开箱即用的基础设置
* 强大易用的任务调度插件
* 快捷的wpf开发组件包含完整的MVVM基础设施
* 开箱即用的数据采集组件

# Packages

|Package Name|Package Icon|Version|
|:-------|:-------:|------:|
|Dnc.Core|[![Dnc.Core/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Dnc.Core/1.1.0.17)|1.1.0.17|
|Dnc.Dispatcher|[![Dnc.Dispatcher/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Dnc.Dispatcher/1.1.0.17)|1.1.0.17|
|Dnc.Data|[![Dnc.Data/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Dnc.Data/1.1.0.16)|1.1.0.17|
|Dnc.SeedWork|[![Dnc.SeedWork/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Dnc.SeedWork/1.1.0.16)|1.1.0.17|
|Dnc.WPF|[![Dnc.WPF/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Dnc.WPF/1.1.0.17)|1.1.0.17|
|Dnc.WPF.UI|[![Dnc.WPF.UI/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Dnc.WPF.UI/1.1.0.17)|1.1.0.17|
|Dnc.Spider|[![Dnc.Spider/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Dnc.Spider/1.1.0.17)|1.1.0.17|
|Dnc.Spider.HttpRequest|[![Dnc.Spider.HttpRequest/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Spider.HttpRequest/1.1.0.17)|1.1.0.17|
|Dnc.Spider.Puppeteer|[![Dnc.Spider.Puppeteer/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Dnc.Spider.Puppeteer/1.1.0.17)|1.1.0.17|
|Dnc.Events|[![Dnc.Events/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Dnc.Events/1.1.0.17)|1.1.0.17|
|Dnc.Events.InMemory|[![Dnc.Events.InMemory/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Dnc.Events.InMemory/1.1.0.17)|1.1.0.17|
|Dnc.Events.RabbitMQ|[![Dnc.Events.RabbitMQ/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Dnc.Events.RabbitMQ/1.1.0.17)|1.1.0.17|
|Dnc.Events.MySql|[![Dnc.Events.MySql/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Dnc.Events.MySql/1.1.0.17)|1.1.0.17|
|Dnc.CQRS|[![Dnc.CQRS/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Dnc.CQRS/1.1.0.17)|1.1.0.17|
|Dnc.Aspects|[![Dnc.Aspects/1.1.0.17](https://img.shields.io/badge/nuget-1.1.0.17-blue.svg)](https://www.nuget.org/packages/Dnc.Aspects/1.1.0.17)|1.1.0.17|

# Getting Started
Fx,代表框架本身兼具框架构建、初始化和解析服务的职责，并提供了一个服务注册事件用于使用者可能需要的自定义服务注册。
Fx.Build()正常服务生成;
Fx.AspectsBuild() 服务注册构建并启用AOP。
```c#
Fx.Construct<FrameworkConstruction>()
  .Build();
Fx.Resolve<TService>();
```

# Development Logs
* 2019-06-05 13:56 HystrixCommandInterceptorAttribute(Aspects)
* 2019-06-05 12:04 Core Construct调整,Configuration 注入放入Construct,而不是构造函数中,使用IPluginInitializer注入Plugin服务
* 2019-06-04 16:38 缓存AOP MemoryCachingInterceptor 开发及测试完成
* 2019-05-31 13:25 release all 1.1.0.17,更新README.md
* 2019-05-30 修复了一个bug,该bug曾导致FrameworkEnvironment获取失败
* 2019-05-29 调整IEventHandlerExecutionContext默认注入，使用者无须关注注入

# More...

* 持续更新中，开发、文档和使用手册尚未完全，敬请期待，如有兴趣请给我Star，您的支持是我继续下去的最大动力...


