using Assets.Code.Items;
using System;
using System.Collections.Generic;
namespace Assets.Code
{
    public class Storage<T> where T:Item
    {
        private Stack<T> _items;
        private int _capacity;       
        public Action ChangeStorage;
        public int Count => _items.Count;
        public Storage(int capacity) 
        {
            _items = new Stack<T>(capacity);
            _capacity = capacity;            
        }
        public void AddItem(T item) 
        {
            if (Count >= _capacity)
                return;
                        

            if (item != null)
            {                
                _items.Push(item);
                ChangeStorage?.Invoke();
            }
            else
            {
                throw new Exception("Attempt to add an empty item!");
            }
        }
        public T Last() 
        {
            if (Count > 0)
            {
                return _items.Peek();
            }
            else
                return default;

        }
        public T GetItem() 
        {
            if (Count >= 0)
            {
                ChangeStorage?.Invoke();
                return _items.Pop();
                                
            }
            else
                return default(T);
        }
        public bool IsFull() =>
            Count >= _capacity;

        public bool IsEmpty() =>
            Count <= 0;        
        
    }
}