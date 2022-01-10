using System.Collections;
using UnityEngine;
using Assets.Code.Items;

namespace Assets.Code.Factory
{
    public class PlasticFactory : MonoBehaviour
    {
        private Storage<Plastic> _productStorage;
        private Storage<Crude> _resourceStorageCrude;
        private Storage<Oil> _resourceStorageOil;
     
        public void Initialization()
        {            
            _productStorage = new Storage<Plastic>(10);
            _resourceStorageCrude = new Storage<Crude>(10);
            _resourceStorageOil = new Storage<Oil>(10);

            Debug.Log($"productStorage {_productStorage}");
            Debug.Log($"resourceStorageCrude {_resourceStorageCrude}");
            Debug.Log($"resourceStorageOil {_resourceStorageOil}");
        }

        public Plastic TakeProduct()
        {
            return _productStorage.GetItem();
        }

        public void PutResource(IResource resource)
        {
            if (resource is Crude)
                _resourceStorageCrude.AddItem(resource as Crude);
            
            if (resource is Oil)
                _resourceStorageOil.AddItem(resource as Oil);
            
        }
    }
}