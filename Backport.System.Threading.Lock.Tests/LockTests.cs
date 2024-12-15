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
        var myLock = new Lock();
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
    /// Scope tests.
    /// </summary>
    [Fact]
    public void Scope()
    {
        var myLock = new Lock();
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
        var myLock = new Lock();
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
    /// Reentrancy test for EnterScope.
    /// </summary>
    [Fact]
    public void ReentrancyTestEnterScope()
    {
        var myLock = new Lock();
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
    /// Reentrancy test for lock.
    /// </summary>
    [Fact]
    public void ReentrancyTestLock()
    {
        var myLock = new Lock();
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
