using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class FocusPointBar : MonoBehaviour
    {
        public Slider sliderFocus;
        public Transform focusPointBarTransform;
        public UIManager uIManager;

        [SerializeField] UIYellowFocusBarPlayer yellowBar;
        [SerializeField] float yellowBarTimer = 2.0f;

        void Start() 
        {
            if (uIManager == null)
            {
                uIManager = FindObjectOfType<UIManager>();
            }
            sliderFocus = GetComponent<Slider>();
            yellowBar = GetComponentInChildren<UIYellowFocusBarPlayer>();
        }

        public void SetMaxFocusPoints(float maxFocusPoints)
        {
            sliderFocus.maxValue = maxFocusPoints;
            sliderFocus.value = maxFocusPoints;

            if (yellowBar != null)
            {
                yellowBar.SetMaxStat(maxFocusPoints);
            }
        }

        public void SetCurrentFocusPoints(float currentFocusPoints)
        {
            uIManager.ShowHUD();
            if (yellowBar != null)
            {
                yellowBar.StartYellowBar();
                yellowBar.timer = yellowBarTimer; // Every time character get hit, timer will reset/renew

                if (currentFocusPoints > sliderFocus.value)
                {
                    yellowBar.slider.value = currentFocusPoints;
                }
            }
            sliderFocus.value = currentFocusPoints;
        }

        public void SetCurrentLength()
        {
            Vector3 currentPosition = focusPointBarTransform.position;
            focusPointBarTransform.position = currentPosition + new Vector3((sliderFocus.maxValue / 16.695f) + (sliderFocus.maxValue / 25.0425f), 0, 0);

            Vector3 currentScale = focusPointBarTransform.localScale;
            focusPointBarTransform.localScale = currentScale + new Vector3((sliderFocus.maxValue / 600) + (sliderFocus.maxValue / 900), 0, 0);
        }
    }
}
