using System.Collections;
using System.Runtime.InteropServices;

namespace RangeExtensions;

[StructLayout(LayoutKind.Auto)]
public readonly partial record struct WhereRange : IEnumerable<int>
{
    private readonly Func<int, bool> _predicate;

    private readonly int _start;
    private readonly int _end;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal WhereRange(Func<int, bool> predicate, int start, int end)
    {
        ThrowHelpers.CheckNull(predicate);

        _predicate = predicate;

        _start = start;
        _end = end;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator()
    {
        return new(_predicate, _start, _end);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator<int> IEnumerable<int>.GetEnumerator() => GetEnumerator();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    [StructLayout(LayoutKind.Auto)]
    public record struct Enumerator : IEnumerator<int>
    {
        private readonly Func<int, bool> _predicate;

        private readonly int _shift;
        private readonly int _end;

        private int _current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Enumerator(Func<int, bool> predicate, int start, int end)
        {
            _predicate = predicate;

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
            while ((_current += _shift) != _end)
            {
                if (_predicate(_current))
                {
                    return true;
                }
            }

            return false;
        }

        public int Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _current;
        }

        object IEnumerator.Current => _current;

#if NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.NoInlining)]
#else
        [DoesNotReturn]
#endif
        public void Reset()
        {
            throw new NotSupportedException();
        }

        public void Dispose() { }
    }
}
