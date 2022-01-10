using System;
using System.Collections.Generic;
namespace Assets.Code
{
    public class Storage<T> where T:IItem
    {
        private List<T> _items;
        private int _capacity;
        private int count;
        public Storage(int capacity) 
        {
            _items = new List<T>(capacity);
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
                _items.Add(item);
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
                var item = _items[count - 1];
                _items.RemoveAt(count-1);
                count--;
                return item;
            }
            else
                return default(T);
        }
     
    }
}