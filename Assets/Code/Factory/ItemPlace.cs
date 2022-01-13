using Assets.Code.Items.CertainItem;
using Assets.Code.Items;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Code.Factory
{
    public class ItemPlace : StoragePlace
    {
        [SerializeField] private List<Transform> _placeTransform = new List<Transform>();

        public void Place(Item item)
        {
            item.transform.SetParent(_parentTransform);
            item.transform.localRotation = Quaternion.Euler(Vector3.zero);
            PlaceToFirstEmptyTransform(item);
        }
        public Transform NearestEmptyPlacePoint()
        {
            for (int i = 0; i < _placeTransform.Count; i++)
                if (_placeTransform[i].childCount == 0)
                    return _placeTransform[i];

            return _placeTransform[0];
        }
        private void PlaceToFirstEmptyTransform(Item item)
        {
            for (int i = 0; i < _placeTransform.Count; i++)
                if (_placeTransform[i].childCount == 0)
                {
                    item.transform.SetParent(_placeTransform[i]);
                    item.transform.position = _placeTransform[i].position;
                    break;
                }
        }
    }
}