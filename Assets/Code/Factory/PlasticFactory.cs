using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Items;
using Assets.Code.Items.CertainItem;
using System.Collections;
using System;
using DG.Tweening;

namespace Assets.Code.Factory
{
    public class PlasticFactory : BaseFactory
    {
        [SerializeField] private int _sizeProductStorage = 27;
        [SerializeField] private int _sizeStorageCrude = 15;
        [SerializeField] private int _sizeStorageOil = 15;
        [SerializeField] private float _timeMovingItem;
        [SerializeField] private ItemPlace _productPlace;
        [SerializeField] private ItemPlace _resourceCrudePlace;
        [SerializeField] private ItemPlace _resourceOilPlace;
        [SerializeField] private LoadArea _resourcesLoadArea;
        [SerializeField] private LoadArea _productLoadArea;
        [SerializeField] private Transform _factoryPoint;

        private Storage<Crude> _resourceStorageCrude;
        private Storage<Oil> _resourceStorageOil;
        private Storage<Plastic> _productStorage;
        private WaitForSeconds _waitTimeProduction;
        private ItemType _wantedResourceType2;
        private void Start()
        {
            StartCreateProduct();
            _productStorage.ChangeStorage += OnChangeProductStorage;
            _resourceStorageCrude.ChangeStorage += OnChangeResourceCrudeStorage;
            _resourceStorageOil.ChangeStorage += OnChangeResourceOilStorage;

        }
        public void Initialization(float timeProduction, GameObject product)
        {
            _timeForProduction = timeProduction;
            _productPrefab = product;
            _creatingType = product.GetComponent<Item>().GetItemType;
            _wantedResourceType = ItemType.Crude;
            _wantedResourceType2 = ItemType.Oil;
            _productStorage = new Storage<Plastic>(_sizeProductStorage);
            _resourceStorageCrude = new Storage<Crude>(_sizeStorageCrude);
            _resourceStorageOil = new Storage<Oil>(_sizeStorageOil);
            _waitTimeProduction = new WaitForSeconds(_timeForProduction);
            _resourcesLoadArea.Initialize(this);
            _productLoadArea.Initialize(this);
        }

        public override bool IsCanTake => IsCanTakeCrude || IsCanTakeOil;
        public bool IsCanTakeCrude => _resourceStorageCrude.IsFull() == false;
        public bool IsCanTakeOil => _resourceStorageOil.IsFull() == false;
        public override bool IsCanGive => _productStorage.IsEmpty() == false;

        public override Transform GetFirstEmptyResourcesPlace => _resourceCrudePlace.NearestEmptyPlacePoint();

        public bool OneOfResourcesFull => _resourceStorageCrude.IsFull() == false || _resourceStorageOil.IsFull() == false;

        public ItemType WantedResourceType2 { get => _wantedResourceType2; }


        public Transform GetFirstEmptyResourcesPlaceByType(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Crude:
                    return _resourceCrudePlace.NearestEmptyPlacePoint();
                case ItemType.Oil:
                    return _resourceOilPlace.NearestEmptyPlacePoint();
                case ItemType.Plastic:
                    throw new Exception($"{itemType} is invalid type in this case!");
                default:
                    throw new Exception($"Error! Unknown itemType {itemType}");
            }
        }      

        public override bool TryPutResources<T>(T resource)
        {
            if (_resourceStorageCrude.IsFull() == false && resource is Crude)
            {
                _resourceStorageCrude.AddItem(resource as Crude);
                _resourceCrudePlace.Place(_resourceStorageCrude.Last());
                return true;
            }

            if (_resourceStorageOil.IsFull() == false && resource is Oil)
            {
                _resourceStorageOil.AddItem(resource as Oil);
                _resourceOilPlace.Place(_resourceStorageOil.Last());
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
        private void OnChangeResourceCrudeStorage()
        {
            if (_isWorking == false)
                StartCreateProduct();

        }
        private void OnChangeResourceOilStorage()
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
            var resourceCrude = _resourceStorageCrude.GetItem();
            resourceCrude.transform.SetParent(null);
            resourceCrude.transform.DOMove(_factoryPoint.position, _timeMovingItem).OnComplete(() => Destroy(resourceCrude.gameObject)); 
            
            var resourceOil = _resourceStorageOil.GetItem();
            resourceOil.transform.SetParent(null);
            resourceOil.transform.DOMove(_factoryPoint.position, _timeMovingItem).OnComplete(() => InstantiateNewProduct(resourceOil.gameObject));

        }
        private void InstantiateNewProduct(GameObject usedResource)
        {
            Destroy(usedResource);
            var newItem = Instantiate(_productPrefab);
            if (newItem.TryGetComponent(out Plastic plastic))
                MoveProductToStorage(plastic);
            else
                throw new Exception($"{newItem} don't nave Plastic component!");

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
            _productStorage.AddItem(itemProduct as Plastic);
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
                FailedToCreate?.Invoke($"PlasticFactory was stopped! Product storage is full!");
                return false;
            }
            if (_resourceStorageCrude.IsEmpty())
            {
                FailedToCreate?.Invoke($"PlasticFactory was stopped! Not enough Crude resource!");
                return false;
            }

            if (_resourceStorageOil.IsEmpty())
            {
                FailedToCreate?.Invoke($"PlasticFactory was stopped! Not enough Oil resource!");
                return false;
            }
            return true;
        }

        private void OnDestroy()
        {
            _productStorage.ChangeStorage -= OnChangeProductStorage;
            _resourceStorageCrude.ChangeStorage -= OnChangeResourceCrudeStorage;
            _resourceStorageOil.ChangeStorage -= OnChangeResourceOilStorage;
        }
    }
}