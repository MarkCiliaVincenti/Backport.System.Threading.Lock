using FluentAssertions;

namespace Tests;

public class LockTests
{
    [Fact]
    public void NormalLock()
    {
        Lock myLock = LockFactory.Create();
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

    [Fact]
    public void Scope()
    {
        Lock myLock = LockFactory.Create();
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

    [Fact]
    public void TryEnter()
    {
        Lock myLock = LockFactory.Create();
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
}