using System;
using System.Collections.Generic;
namespace Assets.Code
{
    public class Storage<T> where T:IItem
    {
        private Stack<T> _items;
        private int _capacity;
        private int count;
        public Action ChangeStorage;
        public Storage(int capacity) 
        {
            _items = new Stack<T>(capacity);
            _capacity = capacity;
            count = 0;
        }
        public void AddItem(T item) 
        {
            if (count >= _capacity)
                return;
                        

            if (item != null)
            {
                count++;
                _items.Push(item);
                ChangeStorage?.Invoke();
            }
            else
            {
                throw new Exception("Attempt to add an empty item!");
            }
        }

        public T GetItem() 
        {
            if (count > 0)
            {
                count--;
                ChangeStorage?.Invoke();
                return _items.Pop();
                                
            }
            else
                return default(T);
        }
        public bool IsFull() =>
            count >= _capacity;

        public bool IsEmpty() =>
            count == 0;
        
        
    }
}