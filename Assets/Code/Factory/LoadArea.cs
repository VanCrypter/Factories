using System.Collections;
using UnityEngine;

namespace Assets.Code.Factory
{
    public class LoadArea : MonoBehaviour
    {
        protected BaseFactory _factory;

        public void Initialize(BaseFactory factory) 
        {
            _factory = factory;
        }
        public BaseFactory GetFactory => _factory;     
        
    }
}