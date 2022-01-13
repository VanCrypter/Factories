using Assets.Code.Items;
using Assets.Code.Items.CertainItem;
using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Assets.Code.Factory
{
    public class CrudeFactory : BaseFactory
    {
        [SerializeField] private int _sizeProductStorage = 18;
        [SerializeField] private float _timeMovingItem;
        [SerializeField] private ItemPlace _productPlace;
        [SerializeField] private LoadArea _productLoadArea;
        [SerializeField] private Transform _factoryPoint;


        private WaitForSeconds _waitTimeProduction;
        protected Storage<Crude> _productStorage;

        public override bool IsCanTake => false;

        public override bool IsCanGive => _productStorage.IsEmpty() == false;

        public override Transform GetFirstEmptyResourcesPlace => throw new Exception("This Factory don't have resource place!");

        public void Initialization(float timeProduction, GameObject product)
        {
            _timeForProduction = timeProduction;
            _productPrefab = product;
            _creatingType = product.GetComponent<Item>().GetItemType;
            _productStorage = new Storage<Crude>(_sizeProductStorage);
            _waitTimeProduction = new WaitForSeconds(_timeForProduction);
            _productLoadArea.Initialize(this);
        }

        private void Start()
        {
            StartCreateProduct();
            _productStorage.ChangeStorage += OnChangeProductStorage;
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
        public override bool TryPutResources<T>(T resource)
        {
            return false;
        }
        private void OnChangeProductStorage()
        {
            if (_isWorking == false)
            {
                StartCreateProduct();
            }
        }

        private void StartCreateProduct()
        {
            StartCoroutine(Creation());
            _isWorking = true;
        }

        private void CreateProduct()
        {
            var product = Instantiate(_productPrefab).GetComponent<Crude>();
            MoveProductToStorage(product);
        }

        private void MoveProductToStorage(Crude product)
        {
            product.transform.position = _factoryPoint.transform.position;
            product.transform.DOMove(_productPlace.NearestEmptyPlacePoint().position, _timeMovingItem)
                .OnComplete(() => AddProduct(product));
        }

        private void AddProduct(Crude product)
        {
            _productStorage.AddItem(product);
            _productPlace.Place(product);
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
                FailedToCreate?.Invoke($"CrudeFactory was stopped! Product storage is full!");
                return false;
            }

            return true;
        }

        private void OnDestroy()
        {
            _productStorage.ChangeStorage -= OnChangeProductStorage;
        }


    }
}