#if !NET48
using System.Buffers;
#endif

using Xunit.Sdk;

namespace RangeExtensions.Tests;

internal static class AssertHelpers
{
    public static void EqualValueOrException<T>(
        Func<T> expectedValueSource,
        Func<T> actualValueSource,
        bool allowInherited = false)
    {
        try
        {
            Assert.Equal(expectedValueSource(), actualValueSource());
        }
        catch (Exception expectedException) when (expectedException is not EqualException)
        {
            var exceptionType = expectedException.GetType();

            if (!allowInherited)
            {
                Assert.Throws(exceptionType, () => expectedValueSource());
                return;
            }

            if (ThrowsSubclassOf(exceptionType, actualValueSource))
            {
                return;
            }

            throw;
        }
    }

    public static void EqualValueOrException<T>(
        Func<IEnumerable<T>> expectedValueSource,
        Func<IEnumerable<T>> actualValueSource,
        bool allowInherited = false)
    {
        try
        {
            Assert.Equal(expectedValueSource(), actualValueSource());
        }
        catch (Exception expectedException) when (expectedException is not EqualException)
        {
            var exceptionType = expectedException.GetType();

            if (!allowInherited)
            {
                Assert.Throws(exceptionType, () => expectedValueSource());
                return;
            }

            if (ThrowsSubclassOf(exceptionType, actualValueSource))
            {
                return;
            }

            throw;
        }
    }

    private static bool ThrowsSubclassOf<T>(Type parentType, Func<T> func)
    {
        try
        {
            func();
            return false;
        }
        catch (Exception actualException)
        {
            return parentType.IsAssignableFrom(actualException.GetType());
        }
    }
}

#if !NET48
internal ref struct Buffer<T>
{
    private readonly T[]? _pooled;

    public readonly Span<T> Span;

    public Buffer(Span<T> initialBuffer, int length)
    {
        if (length > initialBuffer.Length)
        {
            _pooled = ArrayPool<T>.Shared.Rent(length);
            Span = _pooled.AsSpan(..length);
        }
        else
        {
            _pooled = default;
            Span = initialBuffer[..length];
        }
    }

    public static implicit operator Span<T>(Buffer<T> buffer) => buffer.Span;

    public void Dispose()
    {
        if (_pooled != null)
        {
            ArrayPool<T>.Shared.Return(_pooled);
        }
    }
}

internal static class Buffer
{
    public static Buffer<T> Create<T>(Span<T> initialBuffer, int length) => new(initialBuffer, length);
}
#endif
