// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;

namespace Benchmarks;

/// <summary>
/// Benchmark class for Backport.System.Threading.Lock.
/// </summary>
[Config(typeof(MemoryConfig))]
[MemoryDiagnoser]
[JsonExporterAttribute.Full]
[JsonExporterAttribute.FullCompressed]
public class Benchmarks
{
#pragma warning disable IDE0330 // Use 'System.Threading.Lock'
    private static readonly object LockObject = new();
#pragma warning restore IDE0330 // Use 'System.Threading.Lock'
    private static readonly Lock LockLock = new();

    /// <summary>
    /// Benchmark for object class.
    /// </summary>
    [Benchmark(Description = "Using object")]
    public void Object()
    {
        for (int i = 0; i < 50_000_000; i++)
        {
            object myLock = new();
            lock (myLock)
            {
                // do nothing
            }
        }
    }

    /// <summary>
    /// Benchmark for System.Threading.Lock.
    /// </summary>
    [Benchmark(Description = "System.Threading.Lock")]
    public void SystemThreadingLock()
    {
        for (int i = 0; i < 50_000_000; i++)
        {
            Lock myLock = new();
            lock (myLock)
            {
                // do nothing
            }
        }
    }

    /// <summary>
    /// Count to 1000 with object.
    /// </summary>
    /// <returns>The count.</returns>
    [Benchmark]
    public async Task<int> CountTo1000WithObject()
    {
        var count = 0;
        var tasks = new Task[10];
        for (var t = 0; t < tasks.Length; t++)
        {
            tasks[t] = Task.Run(() =>
            {
                // Each task counts to 100, summing up to 1000
                for (var i = 0; i < 100; i++)
                {
                    lock (LockObject)
                    {
                        count++;
                    }
                }
            });
        }

        await Task.WhenAll(tasks);
        return count;
    }

    /// <summary>
    /// Count to 1000 with Lock class.
    /// </summary>
    /// <returns>The count.</returns>
    [Benchmark]
    public async Task<int> CountTo1000WithLockClass()
    {
        var count = 0;
        var tasks = new Task[10];
        for (var t = 0; t < tasks.Length; t++)
        {
            tasks[t] = Task.Run(() =>
            {
                // Each task counts to 100, summing up to 1000
                for (var i = 0; i < 100; i++)
                {
                    lock (LockLock)
                    {
                        count++;
                    }
                }
            });
        }

        await Task.WhenAll(tasks);
        return count;
    }

    private class MemoryConfig : ManualConfig
    {
        public MemoryConfig()
        {
            this.AddDiagnoser(MemoryDiagnoser.Default);
        }
    }
}
