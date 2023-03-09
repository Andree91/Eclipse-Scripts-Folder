using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class LevelUpUI : MonoBehaviour
    {
        public PlayerManager playerManager;
        public Button confirmLevelUpButton;

        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public FocusPointBar focusPointBar;

        [Header("Player Level")]
        public int currentPlayerLevel; //Current level we are before leveling up
        public int projectedPlayerLevel; //The Possible Level which we are going to be after level up
        public TextMeshProUGUI currentPlayerLevelText; //The UI text for the number of current level
        public TextMeshProUGUI projectedPlayerLevelText; //The UI text for the number of projected level

        [Header("Souls")]
        public TextMeshProUGUI currentSoulsText; //Number of souls we are holding right now
        public TextMeshProUGUI soulsRequiredToLevelUpText; //How many souls it cost level up
        int soulsRequiredToLevelUp;
        public int baseLevelUpCost = 5;

        [Header("Health")]
        public Slider healthSlider;
        public TextMeshProUGUI currentHealthLevelText;
        public TextMeshProUGUI projectedHealthLevelText;

        [Header("Stamina")]
        public Slider staminaSlider;
        public TextMeshProUGUI currentStaminaLevelText;
        public TextMeshProUGUI projectedStaminaLevelText;

        [Header("Focus")]
        public Slider focusSlider;
        public TextMeshProUGUI currentFocusLevelText;
        public TextMeshProUGUI projectedFocusLevelText;

        [Header("Strenght")]
        public Slider strenghtSlider;
        public TextMeshProUGUI currentStrenghtLevelText;
        public TextMeshProUGUI projectedStrenghtLevelText;

        [Header("Dexterity")]
        public Slider dexteritySlider;
        public TextMeshProUGUI currentDexterityLevelText;
        public TextMeshProUGUI projectedDexterityLevelText;

        [Header("Intelligence")]
        public Slider intelligenceSlider;
        public TextMeshProUGUI currentIntelligenceLevelText;
        public TextMeshProUGUI projectedIntelligenceLevelText;

        [Header("Faith")]
        public Slider faithSlider;
        public TextMeshProUGUI currentFaithLevelText;
        public TextMeshProUGUI projectedFaithLevelText;

        [Header("Arcane")]
        public Slider arcaneSlider;
        public TextMeshProUGUI currentArcaneLevelText;
        public TextMeshProUGUI projectedArcaneLevelText;

        //Update all UI stats to player current stats
        void OnEnable() 
        {
            currentPlayerLevel = playerManager.playerStatsManager.playerLevel;
            currentPlayerLevelText.text = currentPlayerLevel.ToString();

            projectedPlayerLevel = playerManager.playerStatsManager.playerLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            healthSlider.value = playerManager.playerStatsManager.healthLevel;
            healthSlider.minValue = playerManager.playerStatsManager.healthLevel;
            healthSlider.maxValue = 99;
            currentHealthLevelText.text = playerManager.playerStatsManager.healthLevel.ToString();
            projectedHealthLevelText.text = playerManager.playerStatsManager.healthLevel.ToString();

            staminaSlider.value = playerManager.playerStatsManager.staminaLevel;
            staminaSlider.minValue = playerManager.playerStatsManager.staminaLevel;
            staminaSlider.maxValue = 99;
            currentStaminaLevelText.text = playerManager.playerStatsManager.staminaLevel.ToString();
            projectedStaminaLevelText.text = playerManager.playerStatsManager.staminaLevel.ToString();

            focusSlider.value = playerManager.playerStatsManager.focusLevel;
            focusSlider.minValue = playerManager.playerStatsManager.focusLevel;
            focusSlider.maxValue = 99;
            currentFocusLevelText.text = playerManager.playerStatsManager.focusLevel.ToString();
            projectedFocusLevelText.text = playerManager.playerStatsManager.focusLevel.ToString();

            strenghtSlider.value = playerManager.playerStatsManager.strenghtLevel;
            strenghtSlider.minValue = playerManager.playerStatsManager.strenghtLevel;
            strenghtSlider.maxValue = 99;
            currentStrenghtLevelText.text = playerManager.playerStatsManager.strenghtLevel.ToString();
            projectedStrenghtLevelText.text = playerManager.playerStatsManager.strenghtLevel.ToString();

            dexteritySlider.value = playerManager.playerStatsManager.dexterityLevel;
            dexteritySlider.minValue = playerManager.playerStatsManager.dexterityLevel;
            dexteritySlider.maxValue = 99;
            currentDexterityLevelText.text = playerManager.playerStatsManager.dexterityLevel.ToString();
            projectedDexterityLevelText.text = playerManager.playerStatsManager.dexterityLevel.ToString();

            intelligenceSlider.value = playerManager.playerStatsManager.intelligenceLevel;
            intelligenceSlider.minValue = playerManager.playerStatsManager.intelligenceLevel;
            intelligenceSlider.maxValue = 99;
            currentIntelligenceLevelText.text = playerManager.playerStatsManager.intelligenceLevel.ToString();
            projectedIntelligenceLevelText.text = playerManager.playerStatsManager.intelligenceLevel.ToString();

            faithSlider.value = playerManager.playerStatsManager.faithLevel;
            faithSlider.minValue = playerManager.playerStatsManager.faithLevel;
            faithSlider.maxValue = 99;
            currentFaithLevelText.text = playerManager.playerStatsManager.faithLevel.ToString();
            projectedFaithLevelText.text = playerManager.playerStatsManager.faithLevel.ToString();

            arcaneSlider.value = playerManager.playerStatsManager.arcaneLevel;
            arcaneSlider.minValue = playerManager.playerStatsManager.arcaneLevel;
            arcaneSlider.maxValue = 99;
            currentArcaneLevelText.text = playerManager.playerStatsManager.arcaneLevel.ToString();
            projectedArcaneLevelText.text = playerManager.playerStatsManager.arcaneLevel.ToString();

            currentSoulsText.text = playerManager.playerStatsManager.currentSoulCount.ToString();

            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

        }

        //Updates the player's stats to the projected stats if enough souls
        public void ConfirmPlayerLevelUpStats()
        {
            playerManager.playerStatsManager.playerLevel = projectedPlayerLevel;
            playerManager. playerStatsManager.healthLevel = Mathf.RoundToInt(healthSlider.value);
            playerManager. playerStatsManager.staminaLevel = Mathf.RoundToInt(staminaSlider.value);
            playerManager.playerStatsManager.focusLevel = Mathf.RoundToInt(focusSlider.value);
            playerManager.playerStatsManager.strenghtLevel = Mathf.RoundToInt(strenghtSlider.value);
            playerManager.playerStatsManager.dexterityLevel = Mathf.RoundToInt(dexteritySlider.value);
            playerManager.playerStatsManager.intelligenceLevel = Mathf.RoundToInt(intelligenceSlider.value);
            playerManager.playerStatsManager.faithLevel = Mathf.RoundToInt(faithSlider.value);
            playerManager.playerStatsManager.arcaneLevel = Mathf.RoundToInt(arcaneSlider.value);

            playerManager.playerStatsManager.maxHealth = playerManager.playerStatsManager.SetMaxHealthFromHealthLevel();
            playerManager.playerStatsManager.currentHealth = playerManager.playerStatsManager.maxHealth;
            playerManager.playerStatsManager.maxStamina = playerManager.playerStatsManager.SetMaxStaminaFromStaminaLevel();
            playerManager.playerStatsManager.currentStamina = playerManager.playerStatsManager.maxStamina;
            playerManager.playerStatsManager.maxFocusPoints = playerManager.playerStatsManager.SetMaxFocusPointsFromFocusLevel();
            playerManager.playerStatsManager.currentFocusPoints = playerManager.playerStatsManager.maxFocusPoints;

            if (projectedPlayerLevel != currentPlayerLevel)
            {
                playerManager.playerStatsManager.currentSoulCount = playerManager.playerStatsManager.currentSoulCount - soulsRequiredToLevelUp;
                playerManager.uIManager.soulCountText.text = playerManager.playerStatsManager.currentSoulCount.ToString();
            }

            healthBar.SetMaxHealth(playerManager.playerStatsManager.maxHealth);
            healthBar.SetCurrentHealth(playerManager.playerStatsManager.maxHealth);
            healthBar.SetCurrentLength();

            staminaBar.SetMaxStamina(playerManager.playerStatsManager.maxStamina);
            staminaBar.SetCurrentStamina(playerManager.playerStatsManager.maxStamina);
            staminaBar.SetCurrentLength();

            focusPointBar.SetMaxFocusPoints(playerManager.playerStatsManager.maxFocusPoints);
            focusPointBar.SetCurrentFocusPoints(playerManager.playerStatsManager.maxFocusPoints);
            focusPointBar.SetCurrentLength();

            playerManager.isSitting = false;
            playerManager.playerWeaponSlotManager.LoadWeaponOnSlot(playerManager.playerInventoryManager.rightWeapon, false);
            if (playerManager.playerWeaponSlotManager.backSlot.currentWeaponModel == null && playerManager.playerWeaponSlotManager.shieldBackSlot.currentWeaponModel == null)
            {
                playerManager.playerWeaponSlotManager.LoadWeaponOnSlot(playerManager.playerInventoryManager.leftWeapon, true);
            }
            playerManager.animal.gameObject.SetActive(false);

            gameObject.SetActive(false);
        }

        public void CancelLevelUp()
        {
            playerManager.isSitting = false;
            playerManager.playerWeaponSlotManager.LoadWeaponOnSlot(playerManager.playerInventoryManager.rightWeapon, false);
            if (playerManager.playerWeaponSlotManager.backSlot.currentWeaponModel == null && playerManager.playerWeaponSlotManager.shieldBackSlot.currentWeaponModel == null)
            {
                playerManager.playerWeaponSlotManager.LoadWeaponOnSlot(playerManager.playerInventoryManager.leftWeapon, true);
            }
            playerManager.animal.gameObject.SetActive(false);

            gameObject.SetActive(false);
        }

        void CalculateSoulCostToLevelUp()
        {
            for (int i = 0; i < projectedPlayerLevel; i++)
            {
                soulsRequiredToLevelUp = soulsRequiredToLevelUp + Mathf.RoundToInt(projectedPlayerLevel * baseLevelUpCost * 1.5f);
            }
        }

        //Updates the projected player's total level bu adding up all the projected level stats
        void UpdateProjectedPlayerLevel()
        {
            soulsRequiredToLevelUp = 0;

            projectedPlayerLevel = currentPlayerLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(healthSlider.value) - playerManager.playerStatsManager.healthLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(staminaSlider.value) - playerManager.playerStatsManager.staminaLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(focusSlider.value) - playerManager.playerStatsManager.focusLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(strenghtSlider.value) - playerManager.playerStatsManager.strenghtLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(dexteritySlider.value) - playerManager.playerStatsManager.dexterityLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(intelligenceSlider.value) - playerManager.playerStatsManager.intelligenceLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(faithSlider.value) - playerManager.playerStatsManager.faithLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(arcaneSlider.value) - playerManager.playerStatsManager.arcaneLevel;

            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            CalculateSoulCostToLevelUp();
            soulsRequiredToLevelUpText.text = soulsRequiredToLevelUp.ToString();

            if (playerManager.playerStatsManager.currentSoulCount < soulsRequiredToLevelUp && projectedPlayerLevel != currentPlayerLevel)
            {
                confirmLevelUpButton.interactable = false;
            }
            else
            {
                confirmLevelUpButton.interactable = true;
            }
        }

        public void UpdateHealthLevelSlider()
        {
            projectedHealthLevelText.text = healthSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateStaminaLevelSlider()
        {
            projectedStaminaLevelText.text = staminaSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateFocusLevelSlider()
        {
            projectedFocusLevelText.text = focusSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateStrenghtLevelSlider()
        {
            projectedStrenghtLevelText.text = strenghtSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateDexterityLevelSlider()
        {
            projectedDexterityLevelText.text = dexteritySlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateIntelligenceLevelSlider()
        {
            projectedIntelligenceLevelText.text = intelligenceSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateFaithLevelSlider()
        {
            projectedFaithLevelText.text = faithSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }

        public void UpdateArcaneLevelSlider()
        {
            projectedArcaneLevelText.text = arcaneSlider.value.ToString();
            UpdateProjectedPlayerLevel();
        }
    }
}
