using UnityEngine;

namespace Assets.Code.Items.CertainItem
{
    public class Plastic : Item, IProduct
    {
        public Plastic(GameObject view)
        {
            _view = view.GetComponent<ItemView>();
        }
    }
}