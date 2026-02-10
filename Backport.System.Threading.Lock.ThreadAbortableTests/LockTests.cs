// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;

namespace Tests;

/// <summary>
/// Lock tests.
/// </summary>
public class LockTests
{
    /// <summary>
    /// Normal lock tests.
    /// </summary>
    [Fact]
    public void NormalLock()
    {
        var myLock = LockFactory.Create();
        myLock.IsHeldByCurrentThread.Should().BeFalse();
        lock (myLock)
        {
            myLock.IsHeldByCurrentThread.Should().BeTrue();
            myLock.Enter();
        }

        myLock.IsHeldByCurrentThread.Should().BeTrue();
        myLock.Exit();
        myLock.IsHeldByCurrentThread.Should().BeFalse();
    }

    /// <summary>
    /// EnterScope tests.
    /// </summary>
    [Fact]
    [Obsolete("This method is a best-effort at hardening against thread aborts, but can theoretically retain lock on pre-.NET 5.0. Use with caution.")]
    public void Scope()
    {
        var myLock = LockFactory.Create();
        myLock.IsHeldByCurrentThread.Should().BeFalse();
        using (myLock.EnterScope())
        {
            myLock.IsHeldByCurrentThread.Should().BeTrue();
            myLock.Enter();
        }

        myLock.IsHeldByCurrentThread.Should().BeTrue();
        myLock.Exit();
        myLock.IsHeldByCurrentThread.Should().BeFalse();
    }

    /// <summary>
    /// TryEnter tests.
    /// </summary>
    [Fact]
    public void TryEnter()
    {
        var myLock = LockFactory.Create();
        myLock.IsHeldByCurrentThread.Should().BeFalse();
        myLock.TryEnter().Should().BeTrue();
        myLock.IsHeldByCurrentThread.Should().BeTrue();
        myLock.TryEnter(0).Should().BeTrue();
        myLock.IsHeldByCurrentThread.Should().BeTrue();
        myLock.TryEnter(TimeSpan.Zero).Should().BeTrue();
        myLock.IsHeldByCurrentThread.Should().BeTrue();
        myLock.Exit();
        myLock.IsHeldByCurrentThread.Should().BeTrue();
        myLock.Exit();
        myLock.IsHeldByCurrentThread.Should().BeTrue();
        myLock.Exit();
        myLock.IsHeldByCurrentThread.Should().BeFalse();
    }

    /// <summary>
    /// Reentrancy tests for EnterScope.
    /// </summary>
    [Fact]
    [Obsolete("This method is a best-effort at hardening against thread aborts, but can theoretically retain lock on pre-.NET 5.0. Use with caution.")]
    public void ReentrancyTestEnterScope()
    {
        var myLock = LockFactory.Create();
        using (myLock.EnterScope())
        {
            myLock.IsHeldByCurrentThread.Should().BeTrue();
            using (myLock.EnterScope())
            {
                myLock.IsHeldByCurrentThread.Should().BeTrue();
            }
        }
    }

    /// <summary>
    /// Reentrancy tests for lock.
    /// </summary>
    [Fact]
    public void ReentrancyTestLock()
    {
        var myLock = LockFactory.Create();
        lock (myLock)
        {
            myLock.IsHeldByCurrentThread.Should().BeTrue();
            lock (myLock)
            {
                myLock.IsHeldByCurrentThread.Should().BeTrue();
            }
        }
    }
}
