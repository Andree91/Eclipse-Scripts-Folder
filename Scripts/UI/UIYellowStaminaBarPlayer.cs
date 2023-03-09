using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class UIYellowStaminaBarPlayer : MonoBehaviour
    {
        public Slider slider;
        public float sprintCoollDown = 0.0f;
        StaminaBar parentStaminaBar;

        public float timer;


        void Awake()
        {
            slider = GetComponent<Slider>();
            parentStaminaBar = GetComponentInParent<StaminaBar>();
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
                // Debug.Log("Yellow slider value is " + slider.value);
                // Debug.Log("Stamina slider value is " + parentStaminaBar.sliderStamina.value);
                if (slider.value > parentStaminaBar.sliderStamina.value)
                {
                    slider.value -= 1f;
                    sprintCoollDown -= 1f;
                }
                else if (slider.value == parentStaminaBar.sliderStamina.value)
                {
                    slider.value = parentStaminaBar.sliderStamina.value;
                    sprintCoollDown = 0.0f;
                    // parentHelthBar.SetDamageText();
                    // gameObject.SetActive(false);
                }
                else if (slider.value < parentStaminaBar.sliderStamina.value)
                {
                    sprintCoollDown = 0.0f;
                    // parentHelthBar.SetDamageText();
                    // gameObject.SetActive(false);
                }
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }

        public void SetMaxStat(float maxStat)
        {
            slider.maxValue = maxStat;
            slider.value = maxStat;
        }
    }
}