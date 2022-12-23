using System.Collections;
using System.Runtime.InteropServices;

namespace RangeExtensions;

[StructLayout(LayoutKind.Auto)]
public readonly partial record struct SelectRange<T> : IList<T>
{
    private readonly Func<int, T> _selector;
    private readonly int _start;
    private readonly int _end;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal SelectRange(Func<int, T> selector, int start, int end)
    {
        ThrowHelpers.CheckNull(selector);

        _selector = selector;
        _start = start;
        _end = end;
    }

    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _start < _end
            ? _end - _start
            : _start - _end;
    }

    public bool IsReadOnly
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => true;
    }

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _selector(new RangeEnumerable(_start, _end)[index]);
        set => throw new NotSupportedException();
    }

    public bool Contains(T item)
    {
        foreach (var i in new RangeEnumerable(_start, _end))
        {
            if (EqualityComparer<T>.Default.Equals(_selector(i), item))
            {
                return true;
            }
        }

        return false;
    }

    public void CopyTo(T[] array, int index)
    {
        if (index < 0 || index > array.Length)
        {
            ThrowHelpers.ArgumentOutOfRange();
        }

        var count = Count;
        if (count > (array.Length - index))
        {
            ThrowHelpers.ArgumentException();
        }

        var enumerator = new RangeEnumerable.Enumerator(_start, _end);
        for (var i = index; i < count; i++)
        {
            enumerator.MoveNext();
            array[i] = _selector(enumerator.Current);
        }
    }

    public int IndexOf(T item)
    {
        var index = 0;
        foreach (var i in new RangeEnumerable(_start, _end))
        {
            if (EqualityComparer<T>.Default.Equals(_selector(i), item))
            {
                return index;
            }

            index++;
        }

        return -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator()
    {
        return new(_selector, _start, _end);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    [StructLayout(LayoutKind.Auto)]
    public record struct Enumerator : IEnumerator<T>
    {
        private readonly Func<int, T> _selector;
        private readonly int _shift;
        private readonly int _end;

        private int _current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Enumerator(Func<int, T> selector, int start, int end)
        {
            _selector = selector;

            if (start < end)
            {
                _shift = 1;
                _current = start - 1;
                _end = end;
            }
            else
            {
                _shift = -1;
                _current = start;
                _end = end - 1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return (_current += _shift) != _end;
        }

        public T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _selector(_current);
        }

        object? IEnumerator.Current => _selector(_current);

        public void Dispose() { }

        public void Reset()
        {
            throw new NotSupportedException();
        }
    }

    public void Add(T item)
    {
        throw new NotSupportedException();
    }

    public void Clear()
    {
        throw new NotSupportedException();
    }

    public void Insert(int index, T item)
    {
        throw new NotImplementedException();
    }

    public bool Remove(T item)
    {
        throw new NotSupportedException();
    }

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }
}
