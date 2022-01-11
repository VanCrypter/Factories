using Assets.Code.Items.CertainItem;
using Assets.Code.Items;
using System;
using UnityEngine;

namespace Assets.Code.Factory
{
    public class ProductPlace : StoragePlace
    {
        [SerializeField] private Vector3 _offsetZ;
        [SerializeField] private Vector3 _offsetX;
        [SerializeField] private Vector3 _offsetY;

        private Vector3 _buffer = Vector3.zero;
        private int x, y, z;
        private void Start()
        {
            _sizePlace =new Vector3Int(3,3,3);
        }
        public void Place(Item newItem)
        {
            newItem.View.transform.SetParent(_parentTransform);  
            newItem.View.transform.localPosition = _anchorTransform.localPosition+ _buffer;
            z++;
            _buffer += _offsetZ;
            if (z==_sizePlace.z)
            {
                x++;
                z = 0;
                _buffer = new Vector3(_buffer.x,_buffer.y,0);
                _buffer += _offsetX;
            }

            if (x == _sizePlace.x)
            {
                y++;
                x = 0;
                z = 0;
                _buffer = new Vector3(0, _buffer.y, 0);
                _buffer += _offsetY;
            }
                      
            
        }
    }


}