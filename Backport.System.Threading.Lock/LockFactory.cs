#if !PRE_NETSTANDARD
using System.Runtime.CompilerServices;
#endif
#if NET9_0_OR_GREATER
using FrameworkNamespace = System.Threading;
#endif

namespace Backport.System.Threading
{
#if NET9_0_OR_GREATER
/// <summary>
/// Represents a factory class for backporting .NET 9.0's System.Threading.Lock to prior framework versions.
/// If your project does not target anything before .NET 5.0, you do not need to use this; simply use <see cref="FrameworkNamespace.Lock"/>.
/// </summary>
public static class LockFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="FrameworkNamespace.Lock"/>. On frameworks prior to .NET 9.0, a different backported class is returned.
    /// If your project does not target anything before .NET 5.0, you do not need to use this; simply use <see cref="FrameworkNamespace.Lock"/>.
    /// </summary>
    /// <returns>An instance of <see cref="FrameworkNamespace.Lock"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FrameworkNamespace.Lock Create() => new();
}
#else
    /// <summary>
    /// Represents a factory class for backporting .NET 9.0's System.Threading.Lock to prior framework versions.
    /// If your project does not target anything before .NET 5.0, you do not need to use this; simply use System.Threading.Lock.
    /// </summary>
#if SOURCE_GENERATOR
internal
#else
public
#endif
    static class LockFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="Lock"/>. On .NET 9.0 or later, an instance of System.Threading.Lock is returned.
        /// If your project does not target anything before .NET 5.0, you do not need to use this; simply use System.Threading.Lock.
        /// </summary>
        /// <returns>An instance of <see cref="Lock"/>.</returns>
#if !PRE_NETSTANDARD
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Lock Create() => new();
    }
#endif
}
