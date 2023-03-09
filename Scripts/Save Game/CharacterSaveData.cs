using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("Character Info")]
        public string characterName;
        public int characterLevel;
        public string characterClass;

        [Header("Stat Levels")]
        public int characterHealth;
        public int characterStamina;
        public int characterMana;
        public int characterStrenght;
        public int characterDexterity;
        public int characterIntelligence;
        public int characterFaith;
        public int characterArcane;
        public int characterPoise;

        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Equipment")]
        public int currentRightHandWeaponID;
        public int currentLeftHandWeaponID;

        public int currentHeadEquipmentID;
        public int currentBodyEquipmentID;
        public int currentHandEquipmentID;
        public int currentLegEquipmentID;

        public int currentRightHandSlot01WeaponID;
        public int currentRightHandSlot02WeaponID;
        public int currentRightHandSlot03WeaponID;
        public int currentLeftHandSlot01WeaponID;
        public int currentLeftHandSlot02WeaponID;
        public int currentLeftHandSlot03WeaponID;

        public int currentAmuletSlot01ID;
        public int currentAmuletSlot02ID;
        public int currentAmuletSlot03ID;
        public int currentAmuletSlot04ID;

        public int currentAmmo01ItemID;
        public int currentAmmo02ItemID;

        public int currentBowArrowSlot01ItemID;
        public int currentBowArrowSlot02ItemID;
        public int currentCrossBowArrowSlot01ItemID;
        public int currentCrossBowArrowSlot02ItemID;

        [Header("Consumables")]
        public int currentConsumableID;

        public int currentQuickSlot01ItemID;
        public int currentQuickSlot02ItemID;
        public int currentQuickSlot03ItemID;
        public int currentQuickSlot04ItemID;
        public int currentQuickSlot05ItemID;
        public int currentQuickSlot06ItemID;
        public int currentQuickSlot07ItemID;
        public int currentQuickSlot08ItemID;
        public int currentQuickSlot09ItemID;
        public int currentQuickSlot10ItemID;

        // [Header("Weapons Inventory")]
        // public int 

        [Header("Consumable Inventory")]
        public List<int> consumableInventoryItemIDs = new List<int>();

        [Header("Weapons Inventory")]
        public List<int> weaponInventoryItemIDs = new List<int>();

        [Header("Helmet Equipment Inventory")]
        public List<int> headInventoryItemIDs = new List<int>();

        [Header("Body Equipment  Inventory")]
        public List<int> bodyInventoryItemIDs = new List<int>();

        [Header("Hand Equipment  Inventory")]
        public List<int> handInventoryItemIDs = new List<int>();

        [Header("Leg Equipment  Inventory")]
        public List<int> legInventoryItemIDs = new List<int>();

        [Header("Ranged Ammo Inventory")]
        public List<int> rangedAmmoInventoryItemIDs = new List<int>();

        [Header("Amulets Inventory")]
        public List<int> amuletInventoryItemIDs = new List<int>();

        [Header("Character Appreance")]
        public int characterNakedHeadIndex;
        public int characterHairIndex;
        public int characterEyebrownsIndex;
        public int characterFacialHairIndex;

        public Color characterHairColor;
        public float hairRedAmount;
        public float hairGreenAmount;
        public float hairBlueAmount;

        public Color characterEyebrownsColor;
        public float eyebrownsRedAmount;
        public float eyebrownsGreenAmount;
        public float eyebrownsBlueAmount;

        public Color characterEyeColor;
        public float eyeRedAmount;
        public float eyeGreenAmount;
        public float eyeBlueAmount;

        public Color characterFacialHairColor;
        public float facialHairRedAmount;
        public float facialHairGreenAmount;
        public float facialHairBlueAmount;

        public Color characterSkinColor;
        public float skinRedAmount;
        public float skinGreenAmount;
        public float skinBlueAmount;

        public Color characterMarksColor;
        public float marksRedAmount;
        public float marksGreenAmount;
        public float marksBlueAmount;

        [Header("Items Looted From World")]
        public SerializebleDictionary<int, bool> itemsInWorld; // The int is the wolrd item ID (not item's own ID) and the bool is if the item has been looted
        public int lastInstantiateLootItemID = 5000;

        [Header("Doors Opened In World")]
        public SerializebleDictionary<int, bool> doorsOpened; //

        [Header("Props Destroid In World")]
        public SerializebleDictionary<int, bool> propsDestroid;

        public CharacterSaveData()
        {
            itemsInWorld = new SerializebleDictionary<int, bool>();
            doorsOpened = new SerializebleDictionary<int, bool>();
            propsDestroid = new SerializebleDictionary<int, bool>();
        }
    }
}
