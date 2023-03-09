using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class StaminaBar : MonoBehaviour
    {
        public Slider sliderStamina;
        public Transform staminaBarTransform;
        public UIManager uIManager;
        public bool isDetucing;

        [SerializeField] UIYellowStaminaBarPlayer yellowBar;
        [SerializeField] float yellowBarTimer = 3.0f;

        void Start()
        {
            if (uIManager == null)
            {
                uIManager = FindObjectOfType<UIManager>();
            }
            sliderStamina = GetComponent<Slider>();
            yellowBar = GetComponentInChildren<UIYellowStaminaBarPlayer>();
        }

        public void SetMaxStamina(float maxStamina)
        {
            sliderStamina.maxValue = maxStamina;
            sliderStamina.value = maxStamina;

            if (yellowBar != null)
            {
                yellowBar.SetMaxStat(maxStamina);
            }
        }

        public void SetCurrentStamina(float currentStamina)
        {
            uIManager.ShowHUD();
            if (yellowBar != null)
            {
                if (isDetucing)
                {
                    yellowBar.StartYellowBar();
                    yellowBar.timer = yellowBarTimer; // Every time character get hit, timer will reset/renew
                    isDetucing = false;
                }

                if (uIManager.player.isSprinting || uIManager.player.isBlocking)
                {
                    yellowBar.sprintCoollDown = 2.0f;
                    yellowBar.timer = 1.0f;
                }

                if (currentStamina > sliderStamina.value && !uIManager.player.isSprinting && yellowBar.sprintCoollDown <= 0)
                {
                    yellowBar.slider.value = currentStamina;
                }
            }

            sliderStamina.value = currentStamina;
        }

        // public void SetCurrentStaminaWhileDetucing(float currentStamina)
        // {
        //     if (yellowBar != null)
        //     {
        //         yellowBar.StartYellowBar();
        //         yellowBar.timer = yellowBarTimer; // Every time character get hit, timer will reset/renew

        //         if (currentStamina > sliderStamina.value)
        //         {
        //             yellowBar.slider.value = currentStamina;
        //         }
        //     }

        //     sliderStamina.value = currentStamina;
        // }

        public void SetCurrentLength()
        {
            Vector3 currentPosition = staminaBarTransform.position;
            staminaBarTransform.position = currentPosition + new Vector3((sliderStamina.maxValue / 16.695f) + (sliderStamina.maxValue / 25.0425f), 0, 0); // Have to find better solution

            Vector3 currentScale = staminaBarTransform.localScale;
            staminaBarTransform.localScale = currentScale + new Vector3((sliderStamina.maxValue / 600) + (sliderStamina.maxValue / 900), 0, 0);
        }
    }
}
