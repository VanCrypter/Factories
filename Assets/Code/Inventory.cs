using Assets.Code.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int _capacity = 10;
        [SerializeField] private List<Transform> _placeTransform = new List<Transform>();
        [SerializeField] private List<Item> _items;
        private Quaternion _rotationInInventory = Quaternion.Euler(new Vector3(0, 0, 90));
        public int Count => _items.Count;
        public int Capacity => _capacity;

        private void Start()
        {
            _items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            if (_items.Count < _capacity)
            {
                _items.Add(item);
                PlaceToFirstEmptyTransform(item);
            }
        }

        public Item GetFirstEqualsItem(ItemType itemType)
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (_items[i].GetItemType.Equals(itemType))
                {
                    var item = _items[i];
                    _items.Remove(item);
                    SortInventory();
                    return item;
                }
            }

            return null;
        }

        public void RemoveItem(Item item)
        {
            _items.Remove(item);
            SortInventory();

        }
        public Item LastItemOnStack()
        {
            return _items[Count - 1];
        }
        public bool HasItemByType(ItemType itemType)
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (_items[i].GetItemType.Equals(itemType))
                    return true;

            }

            return false;
        }

        public Item GetFirstItemOfSomeOptions(ItemType itemType1, ItemType itemType2)
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (_items[i].GetItemType.Equals(itemType1) || _items[i].GetItemType.Equals(itemType2))
                {
                    var item = _items[i];
                    _items.RemoveAt(i);
                    SortInventory();
                    return item;
                }
            }
            return null;
        }
        public Transform NearestTransformInventoryPoint()
        {
            foreach (var point in _placeTransform)
            {
                if (point.childCount == 0)
                {
                    return point;
                }
            }

            return _placeTransform[0];

        }

        private void PlaceToFirstEmptyTransform(Item item)
        {
            for (int i = 0; i < _placeTransform.Count; i++)
            {
                if (_placeTransform[i].childCount == 0)
                {
                    item.transform.SetParent(_placeTransform[i]);
                    item.transform.position = _placeTransform[i].position;
                    item.transform.localRotation = _rotationInInventory;
                    break;
                }
            }
        }
        private void SortInventory()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].transform.parent = _placeTransform[i];
                _items[i].transform.position = _placeTransform[i].position;

            }
        }

    }
}