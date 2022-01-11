using System.Collections;
using UnityEngine;

namespace Assets.Code.Items
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Material _material;

        private void Start()
        {
            _renderer.material = _material;
        }

        public void RemoveItem() 
        {
            Destroy(gameObject);
        }
    }
}