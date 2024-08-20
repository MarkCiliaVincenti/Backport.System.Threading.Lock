# ![Backport.System.Threading.Lock](https://raw.githubusercontent.com/MarkCiliaVincenti/Backport.System.Threading.Lock/master/logo32.png) Backport.System.Threading.Lock
[![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/MarkCiliaVincenti/Backport.System.Threading.Lock/dotnet.yml?branch=master&logo=github&style=flat)](https://actions-badge.atrox.dev/MarkCiliaVincenti/Backport.System.Threading.Lock/goto?ref=master) [![NuGet](https://img.shields.io/nuget/v/Backport.System.Threading.Lock?label=NuGet&logo=nuget&style=flat)](https://www.nuget.org/packages/Backport.System.Threading.Lock) [![NuGet](https://img.shields.io/nuget/dt/Backport.System.Threading.Lock?logo=nuget&style=flat)](https://www.nuget.org/packages/Backport.System.Threading.Lock) [![Codacy Grade](https://img.shields.io/codacy/grade/46617e1e645948ce9799026c84b7f1e1?style=flat)](https://app.codacy.com/gh/MarkCiliaVincenti/Backport.System.Threading.Lock/dashboard) [![Codecov](https://img.shields.io/codecov/c/github/MarkCiliaVincenti/Backport.System.Threading.Lock?label=coverage&logo=codecov&style=flat)](https://app.codecov.io/gh/MarkCiliaVincenti/Backport.System.Threading.Lock)

A micro-library that backports .NET 9.0+'s `System.Threading.Lock` to prior framework versions (from .NET Framework 3.5 up to .NET 8.0), providing as much backward compatibility as possible.

## Usage
Use this library the same way you would use [System.Threading.Lock](https://learn.microsoft.com/en-us/dotnet/api/system.threading.lock?view=net-9.0).

In order to get the performance benefits of `System.Threading.Lock`, you must however [multi-target frameworks](https://learn.microsoft.com/en-us/nuget/create-packages/multiple-target-frameworks-project-file) in your `.csproj` file.

Example:
```
<TargetFrameworks>netstandard2.0;net9.0</TargetFrameworks>
```

There is also no need to reference this library as a dependency for .NET 9.0+. You can achieve that by having this in your `.csproj` file:

```
<ItemGroup Condition="!$([MSBuild]::IsTargetFrameworkCompatible('$(TargetFramework)', 'net9.0'))">
  <PackageReference Include="Backport.System.Threading.Lock" Version="1.1.2" />  
</ItemGroup>
```

## Performance
This library was [benchmarked](https://github.com/MarkCiliaVincenti/Backport.System.Threading.Lock/tree/master/Backport.System.Threading.Lock.Benchmarks) against locking on an object on .NET 8.0 and no speed or memory allocation difference was noted, whereas when .NET 9.0 was used the performance was ~25% better as opposed to locking on an object.

## Credits
Check out our [list of contributors](https://github.com/MarkCiliaVincenti/Backport.System.Threading.Lock/blob/master/CONTRIBUTORS.md)!
