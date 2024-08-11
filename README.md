# ![Backport.System.Threading.Lock](https://raw.githubusercontent.com/MarkCiliaVincenti/Backport.System.Threading.Lock/master/logo32.png) Backport.System.Threading.Lock
[![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/MarkCiliaVincenti/Backport.System.Threading.Lock/dotnet.yml?branch=master&logo=github&style=flat)](https://actions-badge.atrox.dev/MarkCiliaVincenti/Backport.System.Threading.Lock/goto?ref=master) [![NuGet](https://img.shields.io/nuget/v/Backport.System.Threading.Lock?label=NuGet&logo=nuget&style=flat)](https://www.nuget.org/packages/Backport.System.Threading.Lock) [![NuGet](https://img.shields.io/nuget/dt/Backport.System.Threading.Lock?logo=nuget&style=flat)](https://www.nuget.org/packages/Backport.System.Threading.Lock) [![Codacy Grade](https://img.shields.io/codacy/grade/50fea66b58154b00b96294e4a3bf1f78?style=flat)](https://app.codacy.com/gh/MarkCiliaVincenti/Backport.System.Threading.Lock/dashboard) [![Codecov](https://img.shields.io/codecov/c/github/MarkCiliaVincenti/Backport.System.Threading.Lock?label=coverage&logo=codecov&style=flat)](https://app.codecov.io/gh/MarkCiliaVincenti/Backport.System.Threading.Lock)

A micro-library that backports .NET 9.0+'s `System.Threading.Lock` to prior versions (from .NET Standard 2.0 up to .NET 8.0), providing as much similar functionality as possible.

## Usage
Use this library the same way you would use [System.Threading.Lock](https://learn.microsoft.com/en-us/dotnet/api/system.threading.lock?view=net-9.0).

In order to get the performance benefits of `System.Threading.Lock`, you must however multi-target frameworks in your `.csproj` file.

Example:
```
<TargetFrameworks>netstandard2.0;net9.0</TargetFrameworks>
```

## Performance
This library was benchmarked against locking on an object on .NET 8.0 and no speed or memory allocation difference was noted.

## Credits
Check out our [list of contributors](https://github.com/MarkCiliaVincenti/Backport.System.Threading.Lock/blob/master/CONTRIBUTORS.md)!
