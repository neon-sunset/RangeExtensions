using System.Collections;

namespace System.Linq;

public readonly record struct SelectEnumerable<T> : IEnumerable<T>
{
    private readonly Func<int, T> _selector;

    private readonly RangeEnumerable _range;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal SelectEnumerable(Func<int, T> selector, RangeEnumerable range)
    {
        ThrowHelpers.CheckNull(selector);

        _selector = selector;
        _range = range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SelectEnumerable<T>.Enumerator GetEnumerator()
    {
        return new(_selector, _range);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public record struct Enumerator : IEnumerator<T>
    {
        private readonly Func<int, T> _selector;

        private RangeEnumerable.Enumerator _rangeEnumerator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Enumerator(Func<int, T> selector, RangeEnumerable range)
        {
            _selector = selector;
            _rangeEnumerator = range.GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return _rangeEnumerator.MoveNext();
        }

        public T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _selector(_rangeEnumerator.Current);
        }

        object? IEnumerator.Current => Current;

        public void Reset()
        {
            throw new NotSupportedException();
        }

        public void Dispose() { }
    }
}
