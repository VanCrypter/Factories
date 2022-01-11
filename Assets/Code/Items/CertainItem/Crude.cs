using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Items.CertainItem
{
    public class Crude : Item, IResource
    {
        public Crude(GameObject view) 
        {
            _view = view.GetComponent<ItemView>();
        }
    }
}