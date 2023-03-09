using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class UIYellowHealthBarPlayer : MonoBehaviour
    {
        public Slider slider;
        HealthBar parentHealthBar;

        public float timer;


        void Awake()
        {
            slider = GetComponent<Slider>();
            parentHealthBar = GetComponentInParent<HealthBar>();
        }

        public void StartYellowBar()
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
                if (slider.value > parentHealthBar.sliderHealth.value)
                {
                    slider.value -= 1f;
                }
                else if (slider.value == parentHealthBar.sliderHealth.value)
                {
                    slider.value = parentHealthBar.sliderHealth.value;
                    // parentHelthBar.SetDamageText();
                    // gameObject.SetActive(false);
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
