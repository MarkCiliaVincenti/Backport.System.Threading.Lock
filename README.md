# ![Backport.System.Threading.Lock](https://raw.githubusercontent.com/MarkCiliaVincenti/Backport.System.Threading.Lock/master/logo32.png)&nbsp;Backport.System.Threading.Lock
[![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/MarkCiliaVincenti/Backport.System.Threading.Lock/dotnet.yml?branch=master&logo=github&style=flat)](https://actions-badge.atrox.dev/MarkCiliaVincenti/Backport.System.Threading.Lock/goto?ref=master) [![NuGet](https://img.shields.io/nuget/v/Backport.System.Threading.Lock?label=NuGet&logo=nuget&style=flat)](https://www.nuget.org/packages/Backport.System.Threading.Lock) [![NuGet](https://img.shields.io/nuget/dt/Backport.System.Threading.Lock?logo=nuget&style=flat)](https://www.nuget.org/packages/Backport.System.Threading.Lock) [![Codacy Grade](https://img.shields.io/codacy/grade/46617e1e645948ce9799026c84b7f1e1?style=flat)](https://app.codacy.com/gh/MarkCiliaVincenti/Backport.System.Threading.Lock/dashboard) [![Codecov](https://img.shields.io/codecov/c/github/MarkCiliaVincenti/Backport.System.Threading.Lock?label=coverage&logo=codecov&style=flat)](https://app.codecov.io/gh/MarkCiliaVincenti/Backport.System.Threading.Lock)

A micro-library that backports/polyfills .NET 9.0+'s `System.Threading.Lock` to prior framework versions (from .NET Framework 3.5 up to .NET 8.0), providing as much backward compatibility as possible. Optionally works as a source generator.

## Why is this useful?
Apart from streamlining locking, especially with a new [lock statement pattern](https://github.com/dotnet/csharplang/issues/7104) being proposed, and the ability to use the [`using` pattern for locking](https://learn.microsoft.com/en-us/dotnet/api/system.threading.lock.enterscope?view=net-9.0#system-threading-lock-enterscope), the more obvious reason for using it is that it gives greater performance (on .NET 9.0+) than simply locking on an object.

## Do I need to add a dependency to my project?
Since version 3.x, Backport.System.Threading.Lock can **optionally work as a source generator**, meaning that a DLL file won't get added to your output. Read further on for more information.

## Why not keep it simple?
Some developers have opted to put in code like this:
```csharp
#if NET9_0_OR_GREATER
global using Lock = System.Threading.Lock;
#else
global using Lock = System.Object;
#endif
```

This is a trick that works in some cases but limits you in what you want to do. You will be unable to use any of the methods offered by `System.Threading.Lock` such as `EnterScope` that allows you to use the using pattern.

More importantly though, if you need to do something like lock in one method and lock with a timeout in another, you simply can't with this code above.

On .NET 8.0 or earlier you cannot do a `myLock.Enter(5)` and on .NET 9.0 or later you wouldn't be able to `Monitor.Enter(myLock, 5)` as this gives you the warning "CS9216: A value of type System.Threading.Lock converted to a different type will use likely unintended monitor-based locking in lock statement."

```csharp
#if NET9_0_OR_GREATER
global using Lock = System.Threading.Lock;
#else
global using Lock = System.Object;
#endif

private readonly Lock myObj = new();

void DoThis()
{
   lock (myObj)
   {
      // do something
   }
}

void DoThat()
{
   myObj.Enter(5); // this will not compile on .NET 8.0
   Monitor.Enter(myObj, 5); // this will immediately enter the lock on .NET 9.0 even if another thread is locking on DoThis()
   // do something else
}
```

If you want to avoid limiting what you are able to do, you need a solution such as this library.

## Usage (as a dependency)
Adding this library as a dependency allows whatever depends on your project to also make use of Backport.System.Threading.Lock.

There are two methods for using this library as a dependency:

1. **Clean method:** If you are only targeting .NET Core 1.0+, .NET 5.0+ or .NET Standard 2.1, then you are strongly recommended to use the clean method.
2. **Factory method:** If you need to target the .NET Framework (and that would also include .NET Standard 2.0), then you need to use the factory method because the clean method cannot be hardened against thread aborts which were removed in .NET Core 1.0 and .NET 5.0.

### Clean method (if only targeting .NET Core 1.0+, .NET 5.0+ or .NET Standard 2.1)
In order to get the performance benefits of `System.Threading.Lock`, you must however [multi-target frameworks](https://learn.microsoft.com/en-us/nuget/create-packages/multiple-target-frameworks-project-file) in your `.csproj` file.

Example:
```xml
<TargetFrameworks>net5.0;net9.0</TargetFrameworks>
```

There is also no need to reference this library as a dependency for .NET 9.0+. You can achieve that by having this in your `.csproj` file:

```xml
<PackageReference Condition="!$([MSBuild]::IsTargetFrameworkCompatible('$(TargetFramework)', 'net9.0'))" Include="Backport.System.Threading.Lock" Version="3.1.2" />
```

Use this library the same way you would use [System.Threading.Lock](https://learn.microsoft.com/en-us/dotnet/api/system.threading.lock?view=net-9.0). Example:

```csharp
private readonly System.Threading.Lock _syncRoot = new();

public void Foo()
{
    lock (_syncRoot)
    {
        // do something
    }
}

public void Bar()
{
    using (_syncRoot.EnterScope())
    {
        // do something
    }
}
```

### Factory method (if targeting the .NET Framework, including .NET Standard 2.0)
Due to the .NET Framewok supporting the notorious `Thread.Abort`, we cannot use the same `System.Threading.Lock` namespace or else the locks would not be hardened against thread aborts, so we need to use a creator method instead.

**IMPORTANT:** You MUST also [multi-target](https://learn.microsoft.com/en-us/nuget/create-packages/multiple-target-frameworks-project-file) .NET 9.0 in your `.csproj` file as well.

Example:
```xml
<TargetFrameworks>netstandard2.0;net9.0</TargetFrameworks>
```

In your `.csproj` file, or ideally in your [Directory.Build.props](https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-by-directory) file to avoid doing it to all projects, do the following:

```xml
<ItemGroup>
  <PackageReference Include="Backport.System.Threading.Lock" Version="3.1.2" />  
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

## Usage (as a source generator)
The usage as a source generator is almost identical to using it as a dependency. The only difference is changing:

```xml
<PackageReference Include="Backport.System.Threading.Lock" Version="3.1.2" />  
```

to:

```xml
<PackageReference Include="Backport.System.Threading.Lock" Version="3.1.2">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>analyzers</IncludeAssets>
</PackageReference>
```

Therefore in the clean method (if only targeting .NET Core 1.0+, .NET 5.0+ or .NET Standard 2.1):

```xml
<PackageReference Condition="!$([MSBuild]::IsTargetFrameworkCompatible('$(TargetFramework)', 'net9.0'))" Include="Backport.System.Threading.Lock" Version="3.1.2">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>analyzers</IncludeAssets>
</PackageReference>
```

and in the factory method (if targeting the .NET Framework, including .NET Standard 2.0):

```xml
<ItemGroup>
  <PackageReference Include="Backport.System.Threading.Lock" Version="3.1.2">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>analyzers</IncludeAssets>
  </PackageReference>
  <Using Condition="$([MSBuild]::IsTargetFrameworkCompatible('$(TargetFramework)', 'net9.0'))" Alias="Lock" Include="System.Threading.Lock" />
  <Using Condition="!$([MSBuild]::IsTargetFrameworkCompatible('$(TargetFramework)', 'net9.0'))" Alias="Lock" Include="Backport.System.Threading.Lock" />
  <Using Alias="LockFactory" Include="Backport.System.Threading.LockFactory" />
</ItemGroup>
```

**IMPORTANT:** You MUST also [multi-target](https://learn.microsoft.com/en-us/nuget/create-packages/multiple-target-frameworks-project-file) .NET 9.0 in your `.csproj` file as well.

## Performance
This library was [benchmarked](https://github.com/MarkCiliaVincenti/Backport.System.Threading.Lock/tree/master/Backport.System.Threading.Lock.Benchmarks) against locking on an object on .NET 8.0 and no speed or memory allocation difference was noted, whereas when .NET 9.0 was used the performance was ~25% better as opposed to locking on an object.

## Credits
Check out our [list of contributors](https://github.com/MarkCiliaVincenti/Backport.System.Threading.Lock/blob/master/CONTRIBUTORS.md)!
