using System.Collections;
using UnityEngine;
using Assets.Code.Factory;
using DG.Tweening;

namespace Assets.Code
{
    public class Game : MonoBehaviour
    {      
        [SerializeField] private CrudeFactory _crudeFactory;      
        [SerializeField] private OilFactory _oilFactory;      
        [SerializeField] private PlasticFactory _plasticFactory;
        [SerializeField] private MessageView _messageView;

        [SerializeField] private GameObject _crudeProduct;
        [SerializeField] private GameObject _oilProduct;
        [SerializeField] private GameObject _plasticProduct;

        private void Awake()
        {
            _crudeFactory.Initialization(.2f,_crudeProduct);
            _crudeFactory.FailedToCreate += _messageView.ShowFailedText; 
            
            _oilFactory.Initialization(2f,_oilProduct);
            _oilFactory.FailedToCreate += _messageView.ShowFailedText;

            _plasticFactory.Initialization(2f,_plasticProduct);
            _plasticFactory.FailedToCreate += _messageView.ShowFailedText;

            DOTween.Init();

        }

        private void OnDestroy()
        {           
            _crudeFactory.FailedToCreate -= _messageView.ShowFailedText;          
            _oilFactory.FailedToCreate -= _messageView.ShowFailedText;          
            _plasticFactory.FailedToCreate -= _messageView.ShowFailedText;
        }

    }
}