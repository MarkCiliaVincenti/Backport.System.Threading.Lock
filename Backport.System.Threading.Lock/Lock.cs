#if (NETSTANDARD2_1_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER) && !NET9_0_OR_GREATER
using System.Runtime.CompilerServices;

namespace System.Threading
{
    /// <summary>
    /// A backport of .NET 9.0+'s System.Threading.Lock. Provides a way to get mutual exclusion in regions of code between different threads.
    /// A lock may be held by one thread at a time.
    /// </summary>
    /// <remarks>
    /// Threads that cannot immediately enter the lock may wait for the lock to be exited or until a specified timeout. A thread
    /// that holds a lock may enter the lock repeatedly without exiting it, such as recursively, in which case the thread should
    /// eventually exit the lock the same number of times to fully exit the lock and allow other threads to enter the lock.
    /// </remarks>
#if SOURCE_GENERATOR
internal
#else
public
#endif
    sealed class Lock
    {
#pragma warning disable CS9216 // A value of type 'System.Threading.Lock' converted to a different type will use likely unintended monitor-based locking in 'lock' statement.
        /// <summary>
        /// <inheritdoc cref="Monitor.Enter(object)"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enter() => Monitor.Enter(this);

        /// <summary>
        /// <inheritdoc cref="Monitor.TryEnter(object)"/>
        /// </summary>
        /// <returns>
        /// <inheritdoc cref="Monitor.TryEnter(object)"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryEnter() => Monitor.TryEnter(this);

        /// <summary>
        /// <inheritdoc cref="Monitor.TryEnter(object, TimeSpan)"/>
        /// </summary>
        /// <returns>
        /// <inheritdoc cref="Monitor.TryEnter(object, TimeSpan)"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryEnter(TimeSpan timeout) => Monitor.TryEnter(this, timeout);

        /// <summary>
        /// <inheritdoc cref="Monitor.TryEnter(object, int)"/>
        /// </summary>
        /// <returns>
        /// <inheritdoc cref="Monitor.TryEnter(object, int)"/>
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryEnter(int millisecondsTimeout) => Monitor.TryEnter(this, millisecondsTimeout);

        /// <summary>
        /// <inheritdoc cref="Monitor.Exit(object)"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="SynchronizationLockException"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Exit() => Monitor.Exit(this);

        /// <summary>
        /// Determines whether the current thread holds this lock.
        /// </summary>
        /// <returns>
        /// true if the current thread holds this lock; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        public bool IsHeldByCurrentThread => Monitor.IsEntered(this);
#pragma warning restore CS9216 // A value of type 'System.Threading.Lock' converted to a different type will use likely unintended monitor-based locking in 'lock' statement.

        /// <summary>
        /// Enters the lock and returns a <see cref="Scope"/> that may be disposed to exit the lock. Once the method returns,
        /// the calling thread would be the only thread that holds the lock. This method is intended to be used along with a
        /// language construct that would automatically dispose the <see cref="Scope"/>, such as with the C# using statement.
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            /// <remarks>
            /// If the calling thread holds the lock multiple times, such as recursively, the lock is exited only once. The
            /// calling thread should ensure that each enter is matched with an exit.
            /// </remarks>
            /// <exception cref="SynchronizationLockException">
            /// The calling thread does not hold the lock.
            /// </exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly void Dispose() => @lock.Exit();
        }
    }
}
#endif
