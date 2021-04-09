using System;

namespace JsonKnownTypes
{
    public class CircuitQueue<T>
    {
        private const int DefaultCapacity = 10;
        
        private T[] _array;

        public int Capacity => _array.Length;

        public int Count => _count;
        
        private int _count = 0;

        public CircuitQueue()
        {
            _array = new T[DefaultCapacity];
        }
        
        public CircuitQueue(int capacity)
        {
            _array = new T[capacity];
        }

        public T this[int index]
        {
            get
            {
                var i = (index + _cursorBegin) % Capacity;
                
                if(_cursorBegin < _cursorEnd && _cursorBegin <= i && i < _cursorEnd)
                    return _array[i];
                if (_cursorEnd <= _cursorBegin && (_cursorBegin <= i || i < _cursorEnd))
                    return _array[i];
                
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        private int _cursorBegin = 0;
        private int _cursorEnd = 0;
        
        public void Enqueue(T item)
        {
            if(_count == Capacity)
                CopyAndIncreaseArray();

            Add(item);
        }

        public void Add(T item)
        {
            _array[_cursorEnd] = item;
            MoveEndCursor();
        }

        private void CopyAndIncreaseArray()
        {
            if (_count != Capacity)
                throw new InvalidOperationException();
            
            var newCapacity = Capacity * 2;
            var newArr = new T[newCapacity];
            if (_cursorBegin == 0)
            {
                Array.Copy(_array, newArr, _count);
            }
            else
            {
                Array.Copy(_array, _cursorBegin, newArr, 0, _count - _cursorBegin);
                Array.Copy(_array, 0, newArr, _count - _cursorBegin, _cursorBegin);
            }

            _cursorBegin = 0;
            _cursorEnd = _count;
            _array = newArr;
        }
        
        public T Dequeue()
        {
            if (_count == 0)
                throw new InvalidOperationException();
            
            var r = _array[_cursorBegin];
            
            MoveBeginCursor();
            return r;
        }

        private void MoveBeginCursor()
        {
            if (_cursorBegin == _cursorEnd)
                throw new InvalidOperationException();

            if (_cursorBegin == Capacity - 1)
                _cursorBegin = 0;
            else
                _cursorBegin++;

            _count--;
        }
        
        private void MoveEndCursor()
        {
            if (_cursorEnd == _cursorBegin && _count != 0)
                throw new InvalidOperationException();

            if (_cursorEnd == Capacity - 1)
                _cursorEnd = 0;
            else
                _cursorEnd++;

            _count++;
        }
    }
}
