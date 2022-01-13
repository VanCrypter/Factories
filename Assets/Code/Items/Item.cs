using System;
using UnityEngine;
using DG.Tweening;
namespace Assets.Code.Items
{
    public class Item :MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Material _material;
        [SerializeField]protected ItemType _itemType;

        private void Start()
        {
            _renderer.material = _material;
        }

        public ItemType GetItemType => _itemType;

        public void MoveTo(Transform endPoint,float duration,TweenCallback OnCompleteTweenCallback = null) 
        {
            transform.DOMove(endPoint.position, duration).OnComplete(OnCompleteTweenCallback);
        }

    }

    public enum ItemType 
    {
        Crude,
        Oil,
        Plastic
    }
}