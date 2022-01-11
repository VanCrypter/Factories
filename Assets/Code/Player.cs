using System.Collections;
using UnityEngine;

namespace Assets.Code
{
  
    public class Player : MonoBehaviour
    {
        [SerializeField]private int _capacity=10;
        private Inventory _inventory;
            

        public void Initialization() 
        {
            _inventory = new Inventory(_capacity);            
        }

        public void PickUpItem() 
        {
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Collision with {collision.collider.name}");
        }


    }
}