using System;
using UnityEngine;
namespace Assets.Code.Items
{
    public abstract class Item : IItem
    {
        protected ItemView _view;     
        public ItemView View { get => _view;}
             
    }
}