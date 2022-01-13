using Assets.Code.Factory;
using Assets.Code.Items;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace Assets.Code
{

    public class Player : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private float _timeTaking = .2f;
        private Coroutine _givingCorutine;
        private Coroutine _takingCorutine;
        private void Awake() =>
            _inventory = GetComponent<Inventory>();

        private void GiveItem(LoadArea takeArea)
        {
            if (_givingCorutine != null)
                StopCoroutine(_givingCorutine);

            _givingCorutine = StartCoroutine(GivingItems(takeArea));
        }

        private void TakeItem(LoadArea givenArea)
        {
            if (_takingCorutine != null)
                StopCoroutine(_takingCorutine);

            _takingCorutine = StartCoroutine(TakingItems(givenArea));

        }

        private IEnumerator GivingItems(LoadArea givenArea)
        {
            if (givenArea.GetFactory.CreatingType == ItemType.Plastic)
            {
                var plasticFactory = givenArea.GetFactory as PlasticFactory;

                while ((_inventory.HasItemByType(plasticFactory.WantedResourceType) && plasticFactory.IsCanTakeCrude) || (_inventory.HasItemByType(plasticFactory.WantedResourceType2) && plasticFactory.IsCanTakeOil))
                {
                    if (plasticFactory.IsCanTakeCrude && (_inventory.HasItemByType(plasticFactory.WantedResourceType)))
                    {
                        var res = _inventory.GetFirstEqualsItem(plasticFactory.WantedResourceType);
                        res.MoveTo(plasticFactory.GetFirstEmptyResourcesPlaceByType(res.GetItemType), _timeTaking, () => GiveItemToFactory(res as IResource, ref givenArea));
                        yield return new WaitForSeconds(_timeTaking);
                        if (plasticFactory.IsCanTakeCrude ==false)
                            break;
                    }

                    if (plasticFactory.IsCanTakeOil && _inventory.HasItemByType(plasticFactory.WantedResourceType2))
                    {
                        var res = _inventory.GetFirstEqualsItem(plasticFactory.WantedResourceType2);
                        res.MoveTo(plasticFactory.GetFirstEmptyResourcesPlaceByType(res.GetItemType), _timeTaking, () => GiveItemToFactory(res as IResource, ref givenArea));
                        yield return new WaitForSeconds(_timeTaking);
                        if (plasticFactory.IsCanTakeOil == false)
                            break;
                    }
                }

            }
            else
            {
                while (givenArea.GetFactory.IsCanTake && _inventory.HasItemByType(givenArea.GetFactory.WantedResourceType))
                {
                    var res = _inventory.GetFirstEqualsItem(givenArea.GetFactory.WantedResourceType);
                    if (givenArea.GetFactory.CreatingType == ItemType.Plastic)
                    {
                        var plasticFactory = givenArea.GetFactory as PlasticFactory;
                        res.MoveTo(plasticFactory.GetFirstEmptyResourcesPlaceByType(res.GetItemType), _timeTaking, () => GiveItemToFactory(res as IResource, ref givenArea));

                    }
                    res.MoveTo(givenArea.GetFactory.GetFirstEmptyResourcesPlace, _timeTaking, () => GiveItemToFactory(res as IResource, ref givenArea));

                    yield return new WaitForSeconds(_timeTaking);
                }
            }
        }
        private void GiveItemToFactory(IResource resource, ref LoadArea loadArea) =>
            loadArea.GetFactory.TryPutResources(resource);

        private IEnumerator TakingItems(LoadArea takeArea)
        {
            while (_inventory.Count < _inventory.Capacity && takeArea.GetFactory.IsCanGive)
            {
                if (takeArea.GetFactory.TryGiveProduct(out IProduct product))
                {
                    var movingProduct = product as Item;
                    movingProduct.MoveTo(_inventory.NearestTransformInventoryPoint(), _timeTaking, () => AddItemToInventory(product));
                    yield return new WaitForSeconds(_timeTaking);
                }
            }
        }

        private void AddItemToInventory(IProduct product) =>
            _inventory.AddItem(product as Item);

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ITakeable takeArea))
                if (takeArea != null)
                    GiveItem(takeArea as LoadArea);

            if (other.TryGetComponent(out IGivingable givenArea))
                if (givenArea != null)
                    TakeItem(givenArea as LoadArea);
        }

        private void OnTriggerExit(Collider other) =>
            StopAllCoroutines();

    }
}