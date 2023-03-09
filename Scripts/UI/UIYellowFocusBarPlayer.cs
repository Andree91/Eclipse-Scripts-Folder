using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class UIYellowFocusBarPlayer : MonoBehaviour
    {
        public Slider slider;
        FocusPointBar parentFocusBar;

        public float timer;

        void Awake()
        {
            slider = GetComponent<Slider>();
            parentFocusBar = GetComponentInParent<FocusPointBar>();
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
            //Debug.Log("Timer is " + timer);
            if (timer <= 0)
            {
                // Debug.Log("Yellow slider value is " + slider.value);
                // Debug.Log("Focus slider value is " + parentFocusBar.sliderFocus.value);
                if (slider.value > parentFocusBar.sliderFocus.value)
                {
                    slider.value -= 1f;
                }
                else if (slider.value == parentFocusBar.sliderFocus.value)
                {
                    slider.value = parentFocusBar.sliderFocus.value;
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