using System.Collections;
using UnityEngine;
using Assets.Code.Factory;

namespace Assets.Code
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private CrudeFactory _firstFactory;
      

        private void Start()
        {         
            _firstFactory.Initialization();
        }


        private void Update()
        {

        }
    }
}