using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class PoisonAmountBar : MonoBehaviour
    {
        public Slider slider;

        void Start()
        {
            slider = GetComponent<Slider>();
            slider.maxValue = 100;
            slider.value = 100;
            gameObject.SetActive(false);
        }

        public void SetCurrentPoisonAmount(int poisonAmount)
        {
            if (poisonAmount > 0)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
            
            slider.value = poisonAmount;
        }
    }
}