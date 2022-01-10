using Assets.Code.Items;
using UnityEngine;

namespace Assets.Code.Factory
{
    public class CrudeFactory :MonoBehaviour
    {
        private StoragePlace _productPlace;
        private Storage<Crude> _productStorage;
        public void Initialization()
        {
            _productStorage = new Storage<Crude>(10);

            Debug.Log($"productStorage {_productStorage}");
        }

        public Crude TakeProduct()
        {
            return _productStorage.GetItem();
        }
    }
}