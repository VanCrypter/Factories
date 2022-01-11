using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Code.Factory
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class MessageView : MonoBehaviour
    {
        private TextMeshProUGUI _textView;

        private void Awake()
        {
            _textView = GetComponent<TextMeshProUGUI>();
        }
        public void ShowFailedText(string text) 
        {
            _textView.text = text;
        }
    
    }
}