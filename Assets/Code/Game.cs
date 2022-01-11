using System.Collections;
using UnityEngine;
using Assets.Code.Factory;

namespace Assets.Code
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private CrudeFactory _crudeFactory;      
        [SerializeField] private OilFactory _oilFactory;      
        [SerializeField] private PlasticFactory _plasticFactory;
        [SerializeField] private MessageView _messageView;

        private void Awake()
        {
            _crudeFactory.Initialization();
            _crudeFactory.FailedToCreate += _messageView.ShowFailedText;          
            _oilFactory.Initialization();
            _oilFactory.FailedToCreate += _messageView.ShowFailedText;
            _plasticFactory.Initialization();
            _plasticFactory.FailedToCreate += _messageView.ShowFailedText;
            _player.Initialization();

        }

        private void OnDestroy()
        {           
            _crudeFactory.FailedToCreate -= _messageView.ShowFailedText;          
            _oilFactory.FailedToCreate -= _messageView.ShowFailedText;          
            _plasticFactory.FailedToCreate -= _messageView.ShowFailedText;
        }

    }
}