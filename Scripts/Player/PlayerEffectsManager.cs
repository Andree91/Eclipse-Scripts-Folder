using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        PlayerManager player;

        PoisonBuildUpBar poisonBuildUpBar;
        public PoisonAmountBar poisonAmountBar;

        public GameObject currentParticleFX; //The particles which play if you are poisoned etc...
        public int amountToBeHealed;
        public int amountToBeRestoredMana;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();

            if (poisonBuildUpBar == null)
            {
                poisonBuildUpBar = FindObjectOfType<PoisonBuildUpBar>();
            }
            if (poisonAmountBar == null)
            {
                poisonAmountBar = FindObjectOfType<PoisonAmountBar>();
            }
        }

        public void HealPlayerFromEffect()
        {
            if (amountToBeHealed != 0)
            {
                player.playerStatsManager.HealCharacter(amountToBeHealed);
                GameObject healParticles = Instantiate(currentParticleFX, player.playerStatsManager.transform);
                Destroy(instantiatedFXModel.gameObject);
                Destroy(healParticles, 2f);
                StartCoroutine(LoadWeaponsOnTimer(1f));
            }
        }


        public void RestoreManaFromEffect()
        {
            if (amountToBeRestoredMana != 0)
            {
                player.playerStatsManager.RestoreCharacterMana(amountToBeRestoredMana);
                GameObject manaParticles = Instantiate(currentParticleFX, player.playerStatsManager.transform);
                Destroy(instantiatedFXModel.gameObject);
                Destroy(manaParticles, 2f);
                StartCoroutine(LoadWeaponsOnTimer(1f));
            }
        }

        public void ClearPoisonFromEffect()
        {
            GameObject clumpParticles = Instantiate(currentParticleFX, player.playerStatsManager.transform);
            Destroy(instantiatedFXModel.gameObject);
            Destroy(clumpParticles, 2f);
            StartCoroutine(LoadWeaponsOnTimer(1f));
        }

        IEnumerator LoadWeaponsOnTimer(float timer)
        {
            yield return new WaitForSeconds(timer);
            player.playerWeaponSlotManager.LoadBothWeaponsOnSlot();
        }

        public void LoadBothWeaponsOnTimer()
        {
            StartCoroutine(LoadWeaponsOnTimer(1f));
        }

        protected override void ProcessBuildUpDecay()
        {
            if (player.characterStatsManager.poisonBuildUp >= 0)
            {
                player.characterStatsManager.poisonBuildUp -= player.playerEffectsManager.buildUpDecayAmount;

                player.uIManager.ShowHUD();
                poisonBuildUpBar.gameObject.SetActive(true);
                poisonBuildUpBar.SetCurrentPoisonBuildUp(Mathf.RoundToInt(player.characterStatsManager.poisonBuildUp));
            }
        }

        // protected override void HandlePoisonBuildUp()
        // {
        //     if (poisonBuildUp <= 0)
        //     {
        //         poisonBuildUpBar.gameObject.SetActive(false);
        //     }
        //     else
        //     {
        //         poisonBuildUpBar.gameObject.SetActive(true);
        //     }
        //     base.HandlePoisonBuildUp();
        //     poisonBuildUpBar.SetCurrentPoisonBuildUp(Mathf.RoundToInt(poisonBuildUp));
        // }

        // protected override void HandleIsPoisonedEffect()
        // {
        //     if (!isPoisoned)
        //     {
        //         poisonAmountBar.gameObject.SetActive(false);
        //     }
        //     else
        //     {
        //         poisonAmountBar.gameObject.SetActive(true);
        //     }
        //     base.HandleIsPoisonedEffect();
        //     poisonAmountBar.SetCurrentPoisonAmount(Mathf.RoundToInt(poisonAmount));
        // }

    }
}