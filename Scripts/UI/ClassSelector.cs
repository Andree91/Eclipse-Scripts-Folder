using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class ClassSelector : MonoBehaviour
    {
        PlayerManager player;
        public GameObject parentGameObject;

        [Header("Class Info UI")]
        // public TextMeshProUGUI playerNameText;
        // public TextMeshProUGUI classText;
        public TextMeshProUGUI playerLevelStat;
        public TextMeshProUGUI healthStat;
        public TextMeshProUGUI staminaStat;
        public TextMeshProUGUI manaStat;
        public TextMeshProUGUI strenghtStat;
        public TextMeshProUGUI dexterityStat;
        public TextMeshProUGUI intelligenceStat;
        public TextMeshProUGUI faithStat;
        public TextMeshProUGUI arcaneStat;
        public TextMeshProUGUI classDescription;

        [Header("Class Summary Info UI")]
        public TextMeshProUGUI classSummaryText;
        public TextMeshProUGUI playerLevelSummaryStat;
        public TextMeshProUGUI healthSummaryStat;
        public TextMeshProUGUI staminaSummaryStat;
        public TextMeshProUGUI manaSummaryStat;
        public TextMeshProUGUI strenghtSummaryStat;
        public TextMeshProUGUI dexteritySummaryStat;
        public TextMeshProUGUI intelligenceSummaryStat;
        public TextMeshProUGUI faithSummaryStat;
        public TextMeshProUGUI arcaneSummaryStat;
        //public TextMeshProUGUI classSummaryDescription;

        //A set of stats of each class
        [Header("Class Starting Stats")]
        public ClassStats[] classStats;

        //A set of gear of each class
        [Header("Class Starting Gear")]
        public ClassGear[] classGears;

        //Set the stats and the gear of selected class

        void Awake()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        void AssignClassStats(int classChosen)
        {
            player.playerStatsManager.className = classStats[classChosen].className;

            //PLAYER LEVEL
            player.playerStatsManager.healthLevel = classStats[classChosen].healthLevel;
            player.playerStatsManager.staminaLevel = classStats[classChosen].staminaLevel;
            player.playerStatsManager.focusLevel = classStats[classChosen].manaLevel;
            player.playerStatsManager.strenghtLevel = classStats[classChosen].strenghtLevel;
            player.playerStatsManager.dexterityLevel = classStats[classChosen].dexterityLevel;
            player.playerStatsManager.intelligenceLevel = classStats[classChosen].intelligenceLevel;
            player.playerStatsManager.faithLevel = classStats[classChosen].faithLevel;
            player.playerStatsManager.arcaneLevel = classStats[classChosen].arcaneLevel;

            player.playerStatsManager.playerLevel = CalculatePlayerLevel();//classStats[classChosen].classLevel;

            healthStat.text = player.playerStatsManager.healthLevel.ToString();
            staminaStat.text = player.playerStatsManager.staminaLevel.ToString();
            manaStat.text = player.playerStatsManager.focusLevel.ToString();
            strenghtStat.text = player.playerStatsManager.strenghtLevel.ToString();
            dexterityStat.text = player.playerStatsManager.dexterityLevel.ToString();
            intelligenceStat.text = player.playerStatsManager.intelligenceLevel.ToString();
            faithStat.text = player.playerStatsManager.faithLevel.ToString();
            arcaneStat.text = player.playerStatsManager.arcaneLevel.ToString();

            playerLevelStat.text = player.playerStatsManager.playerLevel.ToString();

            classDescription.text = classStats[classChosen].classDescription;

            classSummaryText.text = player.playerStatsManager.className;
            playerLevelSummaryStat.text = playerLevelStat.text;
            healthSummaryStat.text = healthStat.text;
            staminaSummaryStat.text = staminaStat.text;
            manaSummaryStat.text = manaStat.text;
            strenghtSummaryStat.text = strenghtStat.text;
            dexteritySummaryStat.text = dexterityStat.text;
            intelligenceSummaryStat.text = intelligenceStat.text;
            faithSummaryStat.text = faithStat.text;
            arcaneSummaryStat.text = arcaneStat.text;
            //classSummaryDescription;
        }

        void AssignClassGear(int classChosen)
        {
            player.playerInventoryManager.currentHelmetEquipment = classGears[classChosen].headEquipment;
            player.playerInventoryManager.currentTorsoEquipment =  classGears[classChosen].chestEquipment;
            player.playerInventoryManager.currentHandEquipment = classGears[classChosen].handEquipment;
            player.playerInventoryManager.currentLegEquipment = classGears[classChosen].legEquipment;
            player.playerEquipmentManager.EquipAllEquipmentModels();

            player.playerInventoryManager.weaponsInRightHandSlots[0] = classGears[classChosen].primaryWeapon;
            player.playerInventoryManager.weaponsInLeftHandSlots[0] = classGears[classChosen].offHandWeapon;
            player.playerInventoryManager.rightWeapon = player.playerInventoryManager.weaponsInRightHandSlots[0];
            player.playerInventoryManager.leftWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[0];
            //player.playerInventoryManager.weaponsInRightHandSlots[1] = classGears[classChosen].secondaryWeapon; //Second Right Hand Weapon
            player.playerWeaponSlotManager.LoadBothWeaponsOnSlot();
            player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.weaponsInRightHandSlots[0], false);
            player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.weaponsInLeftHandSlots[0], true);
        }

        public void AssignKnightClass()
        {
            //Assign Stats
            AssignClassStats(0);

            

            //Assing Gear
            AssignClassGear(0);
            player.gearIsOn = true;
        }

        public void AssignNakedClass()
        {
            //Assign Stats
            AssignClassStats(1);

            //Assing Gear
            AssignClassGear(1);
            player.gearIsOn = false;
        }

        int CalculatePlayerLevel()
        {
            int playerLevel;

            playerLevel = player.playerStatsManager.healthLevel
                        + player.playerStatsManager.staminaLevel
                        + player.playerStatsManager.focusLevel
                        + player.playerStatsManager.strenghtLevel
                        + player.playerStatsManager.dexterityLevel
                        + player.playerStatsManager.intelligenceLevel
                        + player.playerStatsManager.faithLevel
                        + player.playerStatsManager.arcaneLevel
                        - 79;
            
            return playerLevel;
        }

        public void SelectPlayerNakedHeadModel()
        {
            for (int i = 0; i < parentGameObject.transform.childCount; i++)
            {
                var child = parentGameObject.transform.GetChild(i).gameObject;

                if (child != null)
                {
                    if (child.activeInHierarchy)
                    {
                        player.playerEquipmentManager.nakedHeadModel = child;
                        break;
                    }
                }
            }
        }
    }
}
