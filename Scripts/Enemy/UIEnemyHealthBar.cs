using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class UIEnemyHealthBar : MonoBehaviour
    {
        public Slider slider;
        public InputHandler inputHandler;

        float timeUntilBarIsHidden = 0;
        UIYellowBar yellowBar;
        [SerializeField] float yellowBarTimer = 3.0f;
        [SerializeField] TextMeshProUGUI damageText;
        [SerializeField] int currentDamageTaken = 0;

        EnemyManager enemy;

        void Awake()
        {
            enemy = GetComponentInParent<EnemyManager>();
            slider = GetComponentInChildren<Slider>();
            yellowBar = GetComponentInChildren<UIYellowBar>();
            inputHandler = FindObjectOfType<InputHandler>();
        }

        // void OnDisable()
        // {
        //     currentDamageTaken = 0; // Going to reset the damage while bar is hidden
        // }

        void Update()
        {
            if (slider != null)
            {
                timeUntilBarIsHidden = timeUntilBarIsHidden - Time.deltaTime;

                if (timeUntilBarIsHidden <= 0 && !inputHandler.player.cameraHandler.currentLockOnTarget == enemy) //&& inputHandler.lockOnFlag == false)
                {
                    timeUntilBarIsHidden = 0;
                    currentDamageTaken = 0; // Going to reset the damage when bar is hidden
                    if (damageText != null)
                    {
                        damageText.text = "";
                    }
                    slider.gameObject.SetActive(false);
                    if (enemy.lockOnUI != null)
                    {
                        enemy.lockOnUI.SetActive(false);
                    }
                }
                else
                {
                    if (!slider.gameObject.activeInHierarchy && (inputHandler.player.cameraHandler.currentLockOnTarget == enemy || timeUntilBarIsHidden > 0))
                    {
                        slider.gameObject.SetActive(true);
                        if (enemy.lockOnUI != null)
                        {
                            enemy.lockOnUI.SetActive(true);
                        }
                    }
                }

                if (slider.value <= 0)
                {
                    Destroy(slider.gameObject);
                }
            }
        }

        void LateUpdate()
        {
            //transform.LookAt(transform.position + Camera.main.transform);
            //transform.LookAt(Camera.main.transform);
            if (slider != null)
            {
                transform.rotation = Quaternion.LookRotation((transform.position - Camera.main.transform.position).normalized);
            }
        }

        public void SetHealth(int health)
        {
            if (yellowBar != null)
            {
                yellowBar.gameObject.SetActive(true); // Triggers the OnEnable function on yellowbar
                yellowBar.timer = yellowBarTimer; // Every time character get hit, timer will reset/renew

                if (health > slider.value)
                {
                    yellowBar.slider.value = health;
                }
            }

            currentDamageTaken += Mathf.RoundToInt(slider.value - health);
            if (damageText != null)
            {
                damageText.text = currentDamageTaken.ToString();
            }

            slider.value = health;
            timeUntilBarIsHidden = 4.0f;
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;

            if (yellowBar != null)
            {
                yellowBar.SetMaxStat(maxHealth);
            }
        }

        public void SetDamageText()
        {
            currentDamageTaken = 0;
            damageText.text = "";
        }
    }
}
