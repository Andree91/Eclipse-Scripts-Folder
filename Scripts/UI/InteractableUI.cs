using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class InteractableUI : MonoBehaviour
    {
        public TextMeshProUGUI interactableText;
        public TextMeshProUGUI itemText;
        public RawImage itemImage;
        public TextMeshProUGUI itemAmountText;

        void OnDisable() 
        {
            if (itemAmountText != null)
            {
                itemAmountText.text = "";
            }
        }
    }
}
