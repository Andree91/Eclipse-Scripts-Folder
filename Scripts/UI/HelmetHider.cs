using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class HelmetHider : MonoBehaviour
    {
        PlayerManager player;
        HelmetEquipment helmet;
        TorsoEquipment body;
        HandEquipment hand;
        LegEquipment leg;

        void Awake() 
        {
            player = FindObjectOfType<PlayerManager>();
        }
        void Update() 
        {
            if (player.characterAppereanceSavingIsReady)
            {
                UnHideBody();
                player.characterCreationCounter += 1;
                player.SaveCharacterDataToCurrentSaveData(ref player.saveGameManager.currentCharacterSaveData);
                player.characterAppereanceSavingIsReady = false;
                player.characterCreationCounter -= 1;
            }
        }

        public void HideHelmet()
        {
            if (player.playerInventoryManager.currentHelmetEquipment != null)
            {
                helmet = player.playerInventoryManager.currentHelmetEquipment;
                player.playerInventoryManager.currentHelmetEquipment = null;
                player.playerEquipmentManager.EquipAllEquipmentModels();
            }
        }

        public void UnHideHelmet()
        {
            if (helmet != null && player.playerStatsManager.className != "Naked")
            {
                player.playerInventoryManager.currentHelmetEquipment = helmet;
                player.playerEquipmentManager.EquipAllEquipmentModels();
            }
        }

        public void HideBody()
        {
            if (player.playerInventoryManager.currentHelmetEquipment != null)
            {
                helmet = player.playerInventoryManager.currentHelmetEquipment;
                player.playerInventoryManager.currentHelmetEquipment = null;
            }

            if (player.playerInventoryManager.currentTorsoEquipment != null)
            {
                body = player.playerInventoryManager.currentTorsoEquipment;
                player.playerInventoryManager.currentTorsoEquipment = null;
            }

            if (player.playerInventoryManager.currentHandEquipment != null)
            {
                hand = player.playerInventoryManager.currentHandEquipment;
                player.playerInventoryManager.currentHandEquipment = null;
            }

            if (player.playerInventoryManager.currentLegEquipment != null)
            {
                leg = player.playerInventoryManager.currentLegEquipment;
                player.playerInventoryManager.currentLegEquipment = null;
            }

            player.playerEquipmentManager.EquipAllEquipmentModels();
        }

        public void UnHideBody()
        {
            if (player.playerStatsManager.className != "Naked")
            {
                if (helmet != null)
                {
                    player.playerInventoryManager.currentHelmetEquipment = helmet;
                }

                if (body != null)
                {
                    player.playerInventoryManager.currentTorsoEquipment = body;
                }

                if (hand != null)
                {
                    player.playerInventoryManager.currentHandEquipment = hand;
                }

                if (leg != null)
                {
                    player.playerInventoryManager.currentLegEquipment = leg;
                }

                player.playerEquipmentManager.EquipAllEquipmentModels();
            }
        }

    }
}
