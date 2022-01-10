using System.Collections;
using UnityEngine;
using Assets.Code.Items;

namespace Assets.Code.Factory
{
    public class OilFactory : MonoBehaviour
    {
        private Storage<Oil> _productStorage;
        private Storage<Crude> _resourceStorage;
        public void Initialization()
        {
            _resourceStorage = new Storage<Crude>(10);
            _productStorage = new Storage<Oil>(10);

            Debug.Log($"productStorage {_productStorage}");
            Debug.Log($"resourceStorage {_resourceStorage}");
        }

        public Oil TakeProduct()
        {
            return _productStorage.GetItem();
        }

        public void PutResource(Crude resource) 
        {
            _resourceStorage.AddItem(resource);
        }

    }
}