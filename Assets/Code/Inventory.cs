using Assets.Code.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class Inventory 
    {
        private int _capacity;
        private List<Item> _items;
        private Transform _startStackPoint;
        private Vector3 _offset;
        private Vector3 _buffer;
        public Inventory(int capacity) 
        {
            _items = new List<Item>();
            _capacity = capacity;
        }

        public void AddItem(Item item) 
        {
            if (_items.Count< _capacity)
            {
                _items.Add(item);
                item.View.transform.SetParent(_startStackPoint);
                item.View.transform.position = _startStackPoint.position + _buffer;
                _buffer += _offset;

            }
        }
    }
}