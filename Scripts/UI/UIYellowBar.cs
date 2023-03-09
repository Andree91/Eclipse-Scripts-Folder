using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{

    public class UIYellowBar : MonoBehaviour
    {
        public Slider slider;
        UIEnemyHealthBar parentHelthBar;

        public float timer;
        

        void Awake() 
        {
            slider = GetComponent<Slider>();
            parentHelthBar = GetComponentInParent<UIEnemyHealthBar>();
        }

        void OnEnable() 
        {
            if (timer <= 0)
            {
                timer = 1.0f; // How long the yellow bar will be waiting, until it goes at the same value as parent sliedr bar
            }
        }

        void Update() 
        {
            if (timer <= 0)
            {
                if (slider.value > parentHelthBar.slider.value)
                {
                    slider.value -= 1f;
                }
                else if (slider.value <= parentHelthBar.slider.value)
                {
                    slider.value = parentHelthBar.slider.value;
                    parentHelthBar.SetDamageText();
                    gameObject.SetActive(false);
                }
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }

        public void SetMaxStat(int maxStat)
        {
            slider.maxValue = maxStat;
            slider.value = maxStat;
        }
    }
}
