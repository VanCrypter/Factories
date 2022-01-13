using Assets.Code.Items;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Factory
{
    public abstract class BaseFactory : MonoBehaviour
    {
        protected float _timeForProduction;
        protected GameObject _productPrefab;
        protected bool _isWorking; 
        protected ItemType _creatingType;
        protected ItemType _wantedResourceType;
        public abstract bool IsCanTake { get; }
        public abstract bool IsCanGive { get;}
        public abstract bool TryPutResources<T>(T resource);        
        public abstract bool TryGiveProduct(out IProduct product);
        public abstract Transform GetFirstEmptyResourcesPlace { get; }
        public ItemType CreatingType => _creatingType;
        public ItemType WantedResourceType => _wantedResourceType;

        public Action<string> FailedToCreate;        
        

    }
}