using System.Collections.Generic;

namespace JsonKnownTypes
{
    public class BufferQueue<T>
    {
        private int _rangeToRemove = 50;
        private int _count = 0;
        private int _cursor = 0;
        
        private readonly List<T> _list = new List<T>();

        public void Enqueue(T item)
        {
            _list.Add(item);
            _count++;
        }
        
        public T Dequeue()
        {
            if (_cursor == _rangeToRemove)
            {
                _list.RemoveRange(0, _rangeToRemove);
                _cursor = 0;
            }
            
            var r = _list[_cursor];
            _cursor++;
            _count--;
            return r;
        }

        public T this[int index] => _list[index + _cursor];

        public void SetRange(int index) 
            => _rangeToRemove = index;
        
        public int Count => _count;
    }
}
