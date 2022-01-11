
using UnityEngine;
namespace Assets.Code.Items.CertainItem
{
    public class Oil : Item, IProduct, IResource
    {
        public Oil(GameObject view)
        {
            _view = view.GetComponent<ItemView>();
        }
    }
}