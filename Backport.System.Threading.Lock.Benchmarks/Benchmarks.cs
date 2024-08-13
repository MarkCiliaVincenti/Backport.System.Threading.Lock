using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;

namespace Benchmarks
{
    [Config(typeof(MemoryConfig))]
    [MemoryDiagnoser]
    [JsonExporterAttribute.Full]
    [JsonExporterAttribute.FullCompressed]
    public class Benchmarks
    {
        private static readonly object lockObject = new();
        private static readonly Lock lockLock = new();

        private class MemoryConfig : ManualConfig
        {
            public MemoryConfig()
            {
                AddDiagnoser(MemoryDiagnoser.Default);
            }
        }

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

        [Benchmark]
        public async Task<int> CountTo1000WithObject()
        {
            var count = 0;
            var tasks = new Task[10];
            for (var t = 0; t < tasks.Length; t++)
            {
                tasks[t] = Task.Run(() =>
                {
                    for (var i = 0; i < 100; i++) // Each task counts to 100, summing up to 1000
                    {
                        lock (lockObject)
                        {
                            count++;
                        }
                    }
                });
            }

            await Task.WhenAll(tasks);
            return count;
        }

        [Benchmark]
        public async Task<int> CountTo1000WithLockClass()
        {
            var count = 0;
            var tasks = new Task[10];
            for (var t = 0; t < tasks.Length; t++)
            {
                tasks[t] = Task.Run(() =>
                {
                    for (var i = 0; i < 100; i++) // Each task counts to 100, summing up to 1000
                    {
                        lock (lockLock)
                        {
                            count++;
                        }
                    }
                });
            }

            await Task.WhenAll(tasks);
            return count;
        }
    }
}
