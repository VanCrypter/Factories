using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Items;
using Assets.Code.Items.CertainItem;
using System.Collections;
using System;

namespace Assets.Code.Factory
{
    public class PlasticFactory : MonoBehaviour
    {
        [SerializeField] private float _timeForProduction = 1;
        [SerializeField] private int _sizeStorage = 27;
        [SerializeField] private int _sizeStorageCrude = 15;
        [SerializeField] private int _sizeStorageOil = 15;
        [SerializeField] private GameObject _itemPrefab;
        /// <summary>
        /// For TESTS!!!
        /// </summary>
        [SerializeField] private GameObject _crudPrefab;
        /// <summary>
        /// For TESTS!!!
        /// </summary>
        [SerializeField] private GameObject _oilPrefab;

        [SerializeField] private ProductPlace _productPlace;
        [SerializeField] private ResourcePlace _resourceCrudePlace;
        [SerializeField] private ResourcePlace _resourceOilPlace;

        private Storage<Plastic> _productStorage;
        private Storage<Crude> _resourceStorageCrude;
        private Storage<Oil> _resourceStorageOil;
        private WaitForSeconds _waitTimeProduction;
        private bool _isWorking = false;

        public Action<string> FailedToCreate;


        public void Initialization()
        {
            _productStorage = new Storage<Plastic>(_sizeStorage);
            _resourceStorageCrude = new Storage<Crude>(_sizeStorageCrude);
            _resourceStorageOil = new Storage<Oil>(_sizeStorageOil);
            _waitTimeProduction = new WaitForSeconds(_timeForProduction);

        }
        private void Start()
        {
            
            StartCreateProduct();
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Crude(Instantiate(_crudPrefab)));
            PutResource(new Oil(Instantiate(_oilPrefab)));
            PutResource(new Oil(Instantiate(_oilPrefab)));
            PutResource(new Oil(Instantiate(_oilPrefab)));
            PutResource(new Oil(Instantiate(_oilPrefab)));
            PutResource(new Oil(Instantiate(_oilPrefab)));
            PutResource(new Oil(Instantiate(_oilPrefab)));
            PutResource(new Oil(Instantiate(_oilPrefab)));
            PutResource(new Oil(Instantiate(_oilPrefab)));
            PutResource(new Oil(Instantiate(_oilPrefab)));
            PutResource(new Oil(Instantiate(_oilPrefab)));
            PutResource(new Oil(Instantiate(_oilPrefab))); 
            PutResource(new Oil(Instantiate(_oilPrefab)));
            PutResource(new Oil(Instantiate(_oilPrefab)));
            PutResource(new Oil(Instantiate(_oilPrefab)));
            PutResource(new Oil(Instantiate(_oilPrefab)));
            _productStorage.ChangeStorage += OnChangeProductStorage;
            _resourceStorageCrude.ChangeStorage += OnChangeResourceCrudeStorage;
            _resourceStorageOil.ChangeStorage += OnChangeResourceOilStorage;
     
        }

        public Plastic TakeProduct()
        {
            return _productStorage.GetItem();
        }

        public void PutResource(IResource resource)
        {
            if (resource is Crude && _resourceStorageCrude.IsFull() == false)
            {
                _resourceStorageCrude.AddItem(resource as Crude);
                _resourceCrudePlace.Place(resource as Crude);                
            }

            if (resource is Oil && _resourceStorageOil.IsFull() == false)
            {
                _resourceStorageOil.AddItem(resource as Oil);
                _resourceOilPlace.Place(resource as Oil);
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
            {
                StartCreateProduct();
            }
        }
        private void OnChangeResourceOilStorage()
        {
            if (_isWorking == false)
            {
                StartCreateProduct();
            }
        }
        private void OnChangeProductStorage()
        {
            if (_isWorking == false)
            {
                StartCreateProduct();
            }
        }

        private void CreateProduct()
        {
            var newItem = new Plastic(Instantiate(_itemPrefab));
            _productStorage.AddItem(newItem);
            _productPlace.Place(newItem);          
            _resourceStorageCrude.GetItem().View.RemoveItem();
            _resourceStorageOil.GetItem().View.RemoveItem();
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