using System.Collections;
using UnityEngine;
using Assets.Code.Items.CertainItem;
using System;
using Assets.Code.Items;
using DG.Tweening;

namespace Assets.Code.Factory
{
    public class OilFactory : BaseFactory
    {
        [SerializeField] private int _sizeStorage = 27;
        [SerializeField] private float _timeMovingItem;
        [SerializeField] private ItemPlace _productPlace;
        [SerializeField] private ItemPlace _resourcePlace;
        [SerializeField] private LoadArea _resourcesLoadArea;
        [SerializeField] private LoadArea _productLoadArea;
        [SerializeField] private Transform _factoryPoint;
        private Storage<Crude> _resourceStorage;
        private WaitForSeconds _waitTimeProduction;
        private Storage<Oil> _productStorage;

        public override bool IsCanTake => _resourceStorage.IsFull() == false;

        public override bool IsCanGive => _productStorage.IsEmpty() == false;

        public override Transform GetFirstEmptyResourcesPlace =>
            _resourcePlace.NearestEmptyPlacePoint();
        public void Initialization(float timeProduction, GameObject product)
        {
            _timeForProduction = timeProduction;
            _productPrefab = product;
            _creatingType = product.GetComponent<Item>().GetItemType;
            _resourceStorage = new Storage<Crude>(_sizeStorage);
            _productStorage = new Storage<Oil>(_sizeStorage);
            _waitTimeProduction = new WaitForSeconds(_timeForProduction);
            _productLoadArea.Initialize(this);
            _resourcesLoadArea.Initialize(this);
        }
        private void Start()
        {
            StartCreateProduct();
            _resourceStorage.ChangeStorage += OnChangeResourcesStorage;
            _productStorage.ChangeStorage += OnChangeProductStorage;
        }

        public override bool TryPutResources<T>(T resource)
        {
            if (_resourceStorage.IsFull() == false)
            {
                _resourceStorage.AddItem(resource as Crude);
                _resourcePlace.Place(_resourceStorage.Last());           
                return true;
            }

            return false;
        }

        public override bool TryGiveProduct(out IProduct product)
        {
            if (_productStorage.IsEmpty() == false)
            {
                product = _productStorage.GetItem();
                return true;
            }
            else
            {
                product = default;
                return false;
            }
        }
        private void StartCreateProduct()
        {
            StartCoroutine(Creation());
            _isWorking = true;
        }

        private void OnChangeResourcesStorage()
        {
            if (_isWorking == false)
                StartCreateProduct();            
        }
        private void OnChangeProductStorage()
        { 
            if (_isWorking == false)
                StartCreateProduct();            
        }

        private void CreateProduct()
        {
            var resource = _resourceStorage.GetItem();
            resource.transform.SetParent(null);
            resource.transform.DOMove(_factoryPoint.position, _timeMovingItem).OnComplete(() => InstantiateNewProduct(resource.gameObject));
        }
        private void InstantiateNewProduct(GameObject usedResource)
        {
            Destroy(usedResource);
            var newItem = Instantiate(_productPrefab);
            if (newItem.TryGetComponent(out Oil oil))
                MoveProductToStorage(oil);
            else
                throw new Exception($"{newItem} don't nave Oil component!");
        }


        private void MoveProductToStorage(IProduct product)
        {
            var itemProduct = product as Item;
            itemProduct.transform.position = _factoryPoint.transform.position;
            itemProduct.transform.DOMove(_productPlace.NearestEmptyPlacePoint().position, _timeMovingItem)
                .OnComplete(() => AddProduct(product));
        }

        private void AddProduct(IProduct product)
        {
            var itemProduct = product as Item;
            _productStorage.AddItem(itemProduct as Oil);
            _productPlace.Place(itemProduct);
        }

        private IEnumerator Creation()
        {
            while (true)
            {
                yield return _waitTimeProduction;

                if (CanCreate())
                {
                    CreateProduct();
                }
                else
                {
                    _isWorking = false;
                    break;
                }
            }
        }

        private bool CanCreate()
        {
            if (_productStorage.IsFull())
            {
                FailedToCreate?.Invoke($"OilFactory was stopped! Product storage is full!");
                return false;
            }
            if (_resourceStorage.IsEmpty())
            {
                FailedToCreate?.Invoke($"OilFactory was stopped! Not enough Crude resource!");
                return false;
            }
            return true;
        }
        private void OnDestroy()
        {
            _resourceStorage.ChangeStorage -= OnChangeResourcesStorage;
            _productStorage.ChangeStorage -= OnChangeProductStorage;
        }

    }
}