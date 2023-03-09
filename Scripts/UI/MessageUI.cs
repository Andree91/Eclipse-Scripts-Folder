using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AG
{
    public class MessageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText = null;

        public void UpdateMessageText(string message)
        {
            messageText.text = message;
        }
    }
}