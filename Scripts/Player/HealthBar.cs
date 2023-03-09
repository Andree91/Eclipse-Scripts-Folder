using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class HealthBar : MonoBehaviour
    {
        public Slider sliderHealth;
        public Transform healthBarTransform;
        public UIManager uIManager;

        [SerializeField] UIYellowHealthBarPlayer yellowBar;
        [SerializeField] float yellowBarTimer = 3.0f;

        void Start()
        {
            if (uIManager == null)
            {
                uIManager = FindObjectOfType<UIManager>();
            }
            sliderHealth = GetComponent<Slider>();
            yellowBar = GetComponentInChildren<UIYellowHealthBarPlayer>();
        }

        public void SetMaxHealth(int maxHealth)
        {
            sliderHealth.maxValue = maxHealth;
            sliderHealth.value = maxHealth;

            if (yellowBar != null)
            {
                yellowBar.SetMaxStat(maxHealth);
            }
        }

        public void SetCurrentHealth(int currentHealth)
        {
            uIManager.ShowHUD();
            if (yellowBar != null)
            {
                yellowBar.StartYellowBar();
                yellowBar.timer = yellowBarTimer; // Every time character get hit, timer will reset/renew

                if (currentHealth > sliderHealth.value)
                {
                    yellowBar.slider.value = currentHealth;
                }
            }

            sliderHealth.value = currentHealth;
        }

        public void SetCurrentLength()
        {
            //Make more logaritmic system, maybe using ^, square etc... 
            Vector3 currentPosition = healthBarTransform.position;
            healthBarTransform.position = currentPosition + new Vector3((sliderHealth.maxValue / 16.695f) + (sliderHealth.maxValue / 25.0425f), 0, 0);

            Vector3 currentScale = healthBarTransform.localScale;
            healthBarTransform.localScale = currentScale + new Vector3((sliderHealth.maxValue / 600) + (sliderHealth.maxValue / 900), 0, 0);
        }
    }
}
