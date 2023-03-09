using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        PlayerManager player;

        [Header("Equipment Model Changers")]
        //Head
        HelmetModelChanger helmetModelChanger;

        //Torso
        TorsoModelChanger torsoModelChanger;
        LeftUpperArmModelChanger leftUpperArmModelChanger;
        RightUpperArmModelChanger rightUpperArmModelChanger;
        LeftElbowPadModelChanger leftElbowPadModelChanger;
        RightElbowPadModelChanger rightElbowPadModelChanger;
        LeftShoulderPadModelChanger leftShoulderPadModelChanger;
        RightShoulderPadModelChanger rightShoulderPadModelChanger;
        
        //Hand
        LeftLowerArmModelChanger leftLowerArmModelChanger;
        LeftHandModelChanger leftHandModelChanger;
        RightLowerArmModelChanger rightLowerArmModelChanger;
        RightHandModelChanger rightHandModelChanger;

        //Leg
        HipModelChanger hipModelChanger;
        LeftLegModelChanger leftLegModelChanger;
        RightLegModelChanger rightLegModelChanger;
        LeftKneePadModelChanger leftKneePadModelChanger;
        RightKneePadModelChanger rightKneePadModelChanger;

        [Header("LANTERN")]
        public GameObject lantern;

        [Header("Facial Features")]
        public GameObject[] facialFeatures;
        //Could separate for Hair, Eyebrows, Facial Hair

        [Header("Default Naked Models")]
        public GameObject nakedHeadModel;
        public string nakedTorsoModel;
        public string nakedHipModel;
        public string nakedLeftLegModel;
        public string nakedRightLegModel;
        public string nakedLeftUpperArmModel;
        public string nakedLeftLowerArmModel;
        public string nakedLeftHandModel;
        public string nakedRightUpperArmModel;
        public string nakedRightLowerArmModel;
        public string nakedRightHandModel;

        void Awake() 
        {
            player = GetComponent<PlayerManager>();
            
            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
            leftUpperArmModelChanger = GetComponentInChildren<LeftUpperArmModelChanger>();
            rightUpperArmModelChanger = GetComponentInChildren<RightUpperArmModelChanger>();
            leftElbowPadModelChanger = GetComponentInChildren<LeftElbowPadModelChanger>();
            rightElbowPadModelChanger = GetComponentInChildren<RightElbowPadModelChanger>();
            leftShoulderPadModelChanger = GetComponentInChildren<LeftShoulderPadModelChanger>();
            rightShoulderPadModelChanger = GetComponentInChildren<RightShoulderPadModelChanger>();
            leftLowerArmModelChanger = GetComponentInChildren<LeftLowerArmModelChanger>();
            leftHandModelChanger = GetComponentInChildren<LeftHandModelChanger>();
            rightLowerArmModelChanger = GetComponentInChildren<RightLowerArmModelChanger>();
            rightHandModelChanger = GetComponentInChildren<RightHandModelChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
            leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
            rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
            leftKneePadModelChanger = GetComponentInChildren<LeftKneePadModelChanger>();
            rightKneePadModelChanger = GetComponentInChildren<RightKneePadModelChanger>();
        }

        void Start() 
        {
            //EquipAllEquipmentModels();
        }

        public void EquipAllEquipmentModels()
        {
            float poisonResistance = 0;

            //Head Equipment
            //Debug.Log("Now We start to equip gear");
            helmetModelChanger.UnEquipAllHelmetModels();

            if (player.playerInventoryManager.currentHelmetEquipment != null)
            {
                if (player.playerInventoryManager.currentHelmetEquipment.hideFacialFeatures)
                {
                    foreach (var feature in facialFeatures)
                    {
                        feature.SetActive(false);
                    }
                }

                nakedHeadModel.SetActive(false);
                //Debug.Log("Now naked head should be false");
                helmetModelChanger.EquipHelmetModelByName(player.playerInventoryManager.currentHelmetEquipment.helmetModelName);
                player.playerStatsManager.physicalDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment.physicalDefence;
                player.playerStatsManager.fireDamageAbsorptionHead = player.playerInventoryManager.currentHelmetEquipment.fireDefence;
                poisonResistance += player.playerInventoryManager.currentHelmetEquipment.poisonResistance;
            }
            else
            {
                //Equip Default Head
                //helmetModelChanger.EquipHelmetModelByName(nakedHeadModel);
                nakedHeadModel.SetActive(true);
                //Debug.Log("Now naked head should be activate");
                player.playerStatsManager.physicalDamageAbsorptionHead = 0;
                player.playerStatsManager.fireDamageAbsorptionHead = 0;

                foreach (var feature in facialFeatures)
                {
                    feature.SetActive(true);
                }
            }

            //Torso Equipment
            torsoModelChanger.UnEquipAllTorsoModels();
            leftUpperArmModelChanger.UnEquipAllArmModelsModels();
            rightUpperArmModelChanger.UnEquipAllArmModelsModels();
            leftElbowPadModelChanger.UnEquipAllElbowPadModels();
            rightElbowPadModelChanger.UnEquipAllElbowPadModels();
            leftShoulderPadModelChanger.UnEquipAllShoulderPadModels();
            rightShoulderPadModelChanger.UnEquipAllShoulderPadModels();

            if (player.playerInventoryManager.currentTorsoEquipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(player.playerInventoryManager.currentTorsoEquipment.torsoModelName);
                leftUpperArmModelChanger.EquipArmModelByName(player.playerInventoryManager.currentTorsoEquipment.leftUpperArmModelName);
                rightUpperArmModelChanger.EquipArmModelByName(player.playerInventoryManager.currentTorsoEquipment.rightUpperArmModelName);
                leftElbowPadModelChanger.EquipElbowPadModelByName(player.playerInventoryManager.currentTorsoEquipment.leftElbowPadModelName);
                rightElbowPadModelChanger.EquipElbowPadModelByName(player.playerInventoryManager.currentTorsoEquipment.rightElbowPadModelName);
                leftShoulderPadModelChanger.EquipShoulderPadModelByName(player.playerInventoryManager.currentTorsoEquipment.leftShoulderPadModelName);
                rightShoulderPadModelChanger.EquipShoulderPadModelByName(player.playerInventoryManager.currentTorsoEquipment.rightShoulderPadModelName);
                player.playerStatsManager.physicalDamageAbsorptionBody = player.playerInventoryManager.currentTorsoEquipment.physicalDefence;
                player.playerStatsManager.fireDamageAbsorptionBody = player.playerInventoryManager.currentTorsoEquipment.fireDefence;
                poisonResistance += player.playerInventoryManager.currentTorsoEquipment.poisonResistance;
            }
            else
            {
                //Equip Default Torso (Naked)
                torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
                leftUpperArmModelChanger.EquipArmModelByName(nakedLeftUpperArmModel);
                rightUpperArmModelChanger.EquipArmModelByName(nakedRightUpperArmModel);
                player.playerStatsManager.physicalDamageAbsorptionBody = 0;
                player.playerStatsManager.fireDamageAbsorptionBody = 0;
            }

            //Hand Equipment
            leftLowerArmModelChanger.UnEquipAllArmModelsModels();
            leftHandModelChanger.UnEquipAllArmModelsModels();
            rightLowerArmModelChanger.UnEquipAllArmModelsModels();
            rightHandModelChanger.UnEquipAllArmModelsModels();

            if (player.playerInventoryManager.currentHandEquipment != null)
            {
                leftLowerArmModelChanger.EquipArmModelByName(player.playerInventoryManager.currentHandEquipment.leftLowerArmModelName);
                leftHandModelChanger.EquipArmModelByName(player.playerInventoryManager.currentHandEquipment.leftHandModelName);
                rightLowerArmModelChanger.EquipArmModelByName(player.playerInventoryManager.currentHandEquipment.rightLowerArmModelName);
                rightHandModelChanger.EquipArmModelByName(player.playerInventoryManager.currentHandEquipment.rightArmModelName);
                player.playerStatsManager.physicalDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment.physicalDefence;
                player.playerStatsManager.fireDamageAbsorptionHands = player.playerInventoryManager.currentHandEquipment.fireDefence;
                poisonResistance += player.playerInventoryManager.currentHandEquipment.poisonResistance;
            }
            else
            {
                leftLowerArmModelChanger.EquipArmModelByName(nakedLeftLowerArmModel);
                leftHandModelChanger.EquipArmModelByName(nakedLeftHandModel);
                rightLowerArmModelChanger.EquipArmModelByName(nakedRightLowerArmModel);
                rightHandModelChanger.EquipArmModelByName(nakedRightHandModel);
                player.playerStatsManager.physicalDamageAbsorptionHands = 0;
                player.playerStatsManager.fireDamageAbsorptionHands = 0;
            }

            //Leg Equipment
            hipModelChanger.UnEquipAllHipModels();
            leftLegModelChanger.UnEquipAllLegModels();
            rightLegModelChanger.UnEquipAllLegModels();
            leftKneePadModelChanger.UnEquipAllKneePadModels();
            rightKneePadModelChanger.UnEquipAllKneePadModels();

            if (player.playerInventoryManager.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(player.playerInventoryManager.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLegModelByName(player.playerInventoryManager.currentLegEquipment.leftLegName);
                rightLegModelChanger.EquipLegModelByName(player.playerInventoryManager.currentLegEquipment.rightLegName);
                leftKneePadModelChanger.EquipKneePadModelByName(player.playerInventoryManager.currentLegEquipment.leftKneePadModelName);
                rightKneePadModelChanger.EquipKneePadModelByName(player.playerInventoryManager.currentLegEquipment.rightKneePadModelName);
                player.playerStatsManager.physicalDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment.physicalDefence;
                player.playerStatsManager.fireDamageAbsorptionLegs = player.playerInventoryManager.currentLegEquipment.fireDefence;
                poisonResistance += player.playerInventoryManager.currentLegEquipment.poisonResistance;
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHipModel);
                leftLegModelChanger.EquipLegModelByName(nakedLeftLegModel);
                rightLegModelChanger.EquipLegModelByName(nakedRightLegModel);
                player.playerStatsManager.physicalDamageAbsorptionLegs = 0;
                player.playerStatsManager.fireDamageAbsorptionLegs = 0;
            }

            player.playerStatsManager.poisonResistance = poisonResistance;
        }
    }
}
