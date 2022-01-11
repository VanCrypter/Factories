using Assets.Code.Items.CertainItem;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code.Factory
{
    public class StoragePlace: MonoBehaviour
    {
        [SerializeField] protected Transform _parentTransform;
        [SerializeField] protected Transform _anchorTransform;
        protected Vector3Int _sizePlace;
               
    }
}