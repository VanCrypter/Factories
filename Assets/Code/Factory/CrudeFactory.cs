using Assets.Code.Items.CertainItem;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Code.Factory
{
    public class CrudeFactory : MonoBehaviour
    {
        [SerializeField] private float _timeForProduction = 1;
        [SerializeField] private int _sizeProductStorage = 18;
        [SerializeField] private ProductPlace _productPlace;
        [SerializeField] private GameObject _itemPrefab;

        private Storage<Crude> _productStorage;
        private WaitForSeconds _waitTimeProduction;
        private bool _isWorking = false;
        public Action<string> FailedToCreate;

        public void Initialization()
        {
            _productStorage = new Storage<Crude>(_sizeProductStorage);
            _waitTimeProduction = new WaitForSeconds(_timeForProduction);    
        }

        private void Start()
        {
            StartCreateProduct();
            _productStorage.ChangeStorage += OnChangeProductStorage;
        }

        private void OnChangeProductStorage()
        {
            if (_isWorking == false)
            {
                StartCreateProduct();
            }
        }

        public Crude TakeProduct()
        {
            return _productStorage.GetItem();
        }

        private void StartCreateProduct()
        {
            StartCoroutine(Creation());
            _isWorking = true;
        }


        private void CreateProduct()
        {
            if (_productStorage.IsFull() == false)
            {
                var newItem = new Crude(Instantiate(_itemPrefab));
                _productStorage.AddItem(newItem);
                _productPlace.Place(newItem);             

            }
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