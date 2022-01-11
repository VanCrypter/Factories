﻿using System.Collections;
using UnityEngine;
using Assets.Code.Items.CertainItem;
using System;

namespace Assets.Code.Factory
{
    public class OilFactory : MonoBehaviour
    {
        [SerializeField] private float _timeForProduction = 1;
        [SerializeField] private int _sizeStorage = 27;
        [SerializeField] private ProductPlace _productPlace;
        [SerializeField] private ResourcePlace _resourcePlace;
        [SerializeField] private GameObject _itemPrefab;
        /// <summary>
        /// For TESTS!
        /// </summary>
        [SerializeField] private GameObject _crudePrefab;

        private Storage<Oil> _productStorage;
        private Storage<Crude> _resourceStorage;
        private WaitForSeconds _waitTimeProduction;
        private bool _isWorking = false;
        public Action<string> FailedToCreate;
        public void Initialization()
        {
            _resourceStorage = new Storage<Crude>(_sizeStorage);
            _productStorage = new Storage<Oil>(_sizeStorage);
            _waitTimeProduction = new WaitForSeconds(_timeForProduction);
        }
        private void Start()
        {
            StartCreateProduct();
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            PutResource(new Crude(Instantiate(_crudePrefab)));
            _resourceStorage.ChangeStorage += OnChangeResourcesStorage;
            _productStorage.ChangeStorage += OnChangeProductStorage;
        }

        public Oil TakeProduct()
        {
            return _productStorage.GetItem();
        }

        public void PutResource(Crude resource)
        {
            if (_resourceStorage.IsFull() == false)
            {
                _resourceStorage.AddItem(resource);
                _resourcePlace.Place(resource);
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
            var newItem = new Oil(Instantiate(_itemPrefab));
            _productStorage.AddItem(newItem);
            _productPlace.Place(newItem);
            _resourceStorage.GetItem().View.RemoveItem();

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