# ![Backport.System.Threading.Lock](https://raw.githubusercontent.com/MarkCiliaVincenti/Backport.System.Threading.Lock/master/logo32.png) Backport.System.Threading.Lock
[![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/MarkCiliaVincenti/Backport.System.Threading.Lock/dotnet.yml?branch=master&logo=github&style=flat)](https://actions-badge.atrox.dev/MarkCiliaVincenti/Backport.System.Threading.Lock/goto?ref=master) [![NuGet](https://img.shields.io/nuget/v/Backport.System.Threading.Lock?label=NuGet&logo=nuget&style=flat)](https://www.nuget.org/packages/Backport.System.Threading.Lock) [![NuGet](https://img.shields.io/nuget/dt/Backport.System.Threading.Lock?logo=nuget&style=flat)](https://www.nuget.org/packages/Backport.System.Threading.Lock) [![Codacy Grade](https://img.shields.io/codacy/grade/46617e1e645948ce9799026c84b7f1e1?style=flat)](https://app.codacy.com/gh/MarkCiliaVincenti/Backport.System.Threading.Lock/dashboard) [![Codecov](https://img.shields.io/codecov/c/github/MarkCiliaVincenti/Backport.System.Threading.Lock?label=coverage&logo=codecov&style=flat)](https://app.codecov.io/gh/MarkCiliaVincenti/Backport.System.Threading.Lock)

A micro-library that backports .NET 9.0+'s `System.Threading.Lock` to prior framework versions (from .NET Framework 3.5 up to .NET 8.0), providing as much backward compatibility as possible.

## Usage (only if targeting frameworks prior to .NET 5.0)
Due to frameworks prior to .NET 5.0 supporting the notorious `Thread.Abort`, we cannot use the same `System.Threading.Lock` namespace or else the locks would not be hardened against thread aborts, so we need to use a creator method instead.

Remember to [multi-target](https://learn.microsoft.com/en-us/nuget/create-packages/multiple-target-frameworks-project-file) .NET 9.0 in your `.csproj` file as well in order to get the performance benefits of `System.Threading.Lock`.

Example:
```
<TargetFrameworks>netstandard2.0;net9.0</TargetFrameworks>
```

In your `.csproj` file, or ideally in your [Directory.Build.props](https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-by-directory) file to avoid doing it to all projects, do the following:

```
<ItemGroup>
  <Using Condition="$([MSBuild]::IsTargetFrameworkCompatible('$(TargetFramework)', 'net9.0'))" Alias="Lock" Include="System.Threading.Lock" />
  <Using Condition="!$([MSBuild]::IsTargetFrameworkCompatible('$(TargetFramework)', 'net9.0'))" Alias="Lock" Include="Backport.System.Threading.Lock" />
  <Using Alias="LockFactory" Include="Backport.System.Threading.LockFactory" />
</ItemGroup>
```

Usage:
```csharp
private readonly Lock _syncRoot = LockFactory.Create();

public void Foo()
{
    lock (_syncRoot)
    {
        // do something
    }
}

public void Bar()
{
    _syncRoot.Enter();
    // do something that cannot crash on a thread that cannot abort
    _syncRoot.Exit();
}
```

Use the `Lock` class the same way you would use [System.Threading.Lock](https://learn.microsoft.com/en-us/dotnet/api/system.threading.lock?view=net-9.0).

## Usage (only if targeting .NET 5.0 or later)
Use this library the same way you would use [System.Threading.Lock](https://learn.microsoft.com/en-us/dotnet/api/system.threading.lock?view=net-9.0).

In order to get the performance benefits of `System.Threading.Lock`, you must however [multi-target frameworks](https://learn.microsoft.com/en-us/nuget/create-packages/multiple-target-frameworks-project-file) in your `.csproj` file.

Example:
```
<TargetFrameworks>net5.0;net9.0</TargetFrameworks>
```

There is also no need to reference this library as a dependency for .NET 9.0+. You can achieve that by having this in your `.csproj` file:

```
<ItemGroup Condition="!$([MSBuild]::IsTargetFrameworkCompatible('$(TargetFramework)', 'net9.0'))">
  <PackageReference Include="Backport.System.Threading.Lock" Version="2.0.1" />  
</ItemGroup>
```

## Performance
This library was [benchmarked](https://github.com/MarkCiliaVincenti/Backport.System.Threading.Lock/tree/master/Backport.System.Threading.Lock.Benchmarks) against locking on an object on .NET 8.0 and no speed or memory allocation difference was noted, whereas when .NET 9.0 was used the performance was ~25% better as opposed to locking on an object.

## Credits
Check out our [list of contributors](https://github.com/MarkCiliaVincenti/Backport.System.Threading.Lock/blob/master/CONTRIBUTORS.md)!
