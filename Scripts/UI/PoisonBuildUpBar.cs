using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class PoisonBuildUpBar : MonoBehaviour
    {
        public Slider slider;

        void OnDisable() 
        {   
            slider.GetComponentInParent<UIManager>().player.isInCombat = false;
        }

        void Start()
        {
            slider = GetComponent<Slider>();
            slider.maxValue = 100;
            slider.value = 0;
            gameObject.SetActive(false);
        }

        public void SetCurrentPoisonBuildUp(int currentPoisonBuildUp)
        {
            slider.value = currentPoisonBuildUp;

            if (currentPoisonBuildUp <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
