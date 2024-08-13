﻿using System.Runtime.CompilerServices;
#if NET9_0_OR_GREATER
[assembly: TypeForwardedTo(typeof(System.Threading.Lock))]
#else
namespace System.Threading;
/// <summary>
/// A backport of .NET 9.0+'s System.Threading.Lock.
/// </summary>
public sealed class Lock
{
#pragma warning disable CS9216 // A value of type 'System.Threading.Lock' converted to a different type will use likely unintended monitor-based locking in 'lock' statement.
/// <summary>
/// <inheritdoc cref="Monitor.Enter(object)"/>
/// </summary>
/// <exception cref="ArgumentNullException"/>
#if NET45_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void Enter() => Monitor.Enter(this);

    /// <summary>
    /// <inheritdoc cref="Monitor.TryEnter(object)"/>
    /// </summary>
    /// <returns>
    /// <inheritdoc cref="Monitor.TryEnter(object)"/>
    /// </returns>
    /// <exception cref="ArgumentNullException"/>
#if NET45_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public bool TryEnter() => Monitor.TryEnter(this);

    /// <summary>
    /// <inheritdoc cref="Monitor.TryEnter(object, TimeSpan)"/>
    /// </summary>
    /// <returns>
    /// <inheritdoc cref="Monitor.TryEnter(object, TimeSpan)"/>
    /// </returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentOutOfRangeException"/>
#if NET45_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public bool TryEnter(TimeSpan timeout) => Monitor.TryEnter(this, timeout);

    /// <summary>
    /// <inheritdoc cref="Monitor.TryEnter(object, int)"/>
    /// </summary>
    /// <returns>
    /// <inheritdoc cref="Monitor.TryEnter(object, int)"/>
    /// </returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentOutOfRangeException"/>
#if NET45_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public bool TryEnter(int millisecondsTimeout) => Monitor.TryEnter(this, millisecondsTimeout);

    /// <summary>
    /// <inheritdoc cref="Monitor.Exit(object)"/>
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="SynchronizationLockException"/>
#if NET45_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public void Exit() => Monitor.Exit(this);

    /// <summary>
    /// <inheritdoc cref="Monitor.IsEntered(object)"/>
    /// </summary>
    /// <returns>
    /// <inheritdoc cref="Monitor.IsEntered(object)"/>
    /// </returns>
    /// <exception cref="ArgumentNullException"/>
#if NET35
    public bool IsHeldByCurrentThread => throw new NotSupportedException("IsHeldByCurrentThread is only supported on .NET Framework 4.5 or greater.");
#else
    public bool IsHeldByCurrentThread => Monitor.IsEntered(this);
#endif
#pragma warning restore CS9216 // A value of type 'System.Threading.Lock' converted to a different type will use likely unintended monitor-based locking in 'lock' statement.

    /// <summary>
    /// Enters the lock and returns a <see cref="Scope"/> that may be disposed to exit the lock. Once the method returns,
    /// the calling thread would be the only thread that holds the lock. This method is intended to be used along with a
    /// language construct that would automatically dispose the <see cref="Scope"/>, such as with the C# <code>using</code>
    /// statement.
    /// </summary>
    /// <returns>
    /// A <see cref="Scope"/> that may be disposed to exit the lock.
    /// </returns>
    /// <remarks>
    /// If the lock cannot be entered immediately, the calling thread waits for the lock to be exited. If the lock is
    /// already held by the calling thread, the lock is entered again. The calling thread should exit the lock, such as by
    /// disposing the returned <see cref="Scope"/>, as many times as it had entered the lock to fully exit the lock and
    /// allow other threads to enter the lock.
    /// </remarks>
#if NET45_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public Scope EnterScope()
    {
        Enter();
        return new Scope(this);
    }

    /// <summary>
    /// A disposable structure that is returned by <see cref="EnterScope()"/>, which when disposed, exits the lock.
    /// </summary>
    public ref struct Scope(Lock @lock)
    {
        /// <summary>
        /// Exits the lock.
        /// </summary>
        /// <exception cref="SynchronizationLockException"/>
#if NET45_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public readonly void Dispose() => @lock.Exit();
    }
}
#endif