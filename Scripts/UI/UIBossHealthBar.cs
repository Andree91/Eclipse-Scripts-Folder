using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class UIBossHealthBar : MonoBehaviour
    {
        public TextMeshProUGUI bossName;
        Slider slider;
        public TextMeshProUGUI damageText;

        void Awake() 
        {
            slider = GetComponentInChildren<Slider>();
            bossName = GetComponentInChildren<TextMeshProUGUI>();
        }

        void Start() 
        {
            SetHealthBarToInactive();
            damageText.text = null;
        }

        public void SetBossName(string name)
        {
            bossName.text = name;
        }

        public void SetUIHealthBarToActive()
        {
            slider.gameObject.SetActive(true);
            bossName.gameObject.SetActive(true);
        }

        public void SetHealthBarToInactive()
        {
            slider.gameObject.SetActive(false);
            bossName.gameObject.SetActive(false);
        }

        public void SetBossMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        public void SetBossCurrentHealth(int currentHealth)
        {
            slider.value = currentHealth;
        }

        public void ShowDealtDamage(int physicalDamage, int fireDamage)
        {
            StartCoroutine(ShowDealtDamageCoroutine(physicalDamage, fireDamage));
            //Add damege combo values
            //Set showtime timer
        }

        IEnumerator ShowDealtDamageCoroutine(int physicalDamage, int fireDamage)
        {
            damageText.text = (physicalDamage + fireDamage).ToString();
            yield return new WaitForSeconds(2f);
            damageText.text = null;
        }

    }
}
