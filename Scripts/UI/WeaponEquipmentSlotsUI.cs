using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class WeaponEquipmentSlotsUI : MonoBehaviour
    {
        UIManager uIManager;

        public Image icon;
        WeaponItem weapon;

        public bool rightHandSlot01;
        public bool rightHandSlot02;
        public bool rightHandSlot03;
        public bool leftHandSlot01;
        public bool leftHandSlot02;
        public bool leftHandSlot03;

        void Awake() 
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(WeaponItem newItem)
        {
            if (newItem != null)
            {
                weapon = newItem;
            }
            
            if (icon != null)
            {
                if (!weapon.isUnarmed)
                {
                    icon.sprite = weapon.itemIcon;
                }
                
                if (icon.sprite != null)
                {
                    icon.enabled = true;
                    gameObject.SetActive(true);
                }
            }
        }

        public void ClearItem()
        {
            weapon = null;
            icon.sprite = null;
            icon.enabled = false;
            //gameObject.SetActive(false);
        }

        public void SelectThisSlot()
        {
            uIManager.ResetAllSelectedSlots();

            if (rightHandSlot01)
            {
                uIManager.rightHand01Selected = true;
                //uIManager.rightHand02Selected = false;
            }
            else if (rightHandSlot02)
            {
                uIManager.rightHand02Selected = true;
            }
            else if (rightHandSlot03)
            {
                uIManager.rightHand03Selected = true;
            }
            else if (leftHandSlot01)
            {
                uIManager.leftHand01Selected = true;
            }
            else if (leftHandSlot02)
            {
                uIManager.leftHand02Selected = true;
            }
            else if (leftHandSlot03)
            {
                uIManager.leftHand03Selected = true;
            }
            uIManager.weaponSlotIsSelected = true;

            uIManager.itemStatsWindowUI.UpdateWeaponItemStats(weapon);
        }

        // public void UnEquipThisItem()
        // {
        //     if (!uIManager.isInInventoryWindow)
        //     {
        //         if (uIManager.rightHand01Selected)
        //         {
        //             if (!uIManager.player.playerInventoryManager.weaponsInRightHandSlots[0].isUnarmed)
        //             {
        //                 uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInRightHandSlots[0]);
        //             }
        //             uIManager.player.playerInventoryManager.weaponsInRightHandSlots[0] = uIManager.player.playerWeaponSlotManager.unarmedWeapon;
        //         }
        //         else if (uIManager.rightHand02Selected)
        //         {
        //             if (!uIManager.player.playerInventoryManager.weaponsInRightHandSlots[1].isUnarmed)
        //             {
        //                 uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInRightHandSlots[1]);
        //             }
        //             uIManager.player.playerInventoryManager.weaponsInRightHandSlots[1] = uIManager.player.playerWeaponSlotManager.unarmedWeapon;
        //             Debug.Log("Name of icon is " + icon.name);
        //             icon.enabled = false;
        //         }
        //         else if (uIManager.rightHand03Selected)
        //         {
        //             if (!uIManager.player.playerInventoryManager.weaponsInRightHandSlots[2].isUnarmed)
        //             {
        //                 uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInRightHandSlots[2]);
        //             }
        //             uIManager.player.playerInventoryManager.weaponsInRightHandSlots[2] = uIManager.player.playerWeaponSlotManager.unarmedWeapon;
        //         }
        //         else if (uIManager.leftHand01Selected)
        //         {
        //             if (!uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[0].isUnarmed)
        //             {
        //                 uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[0]);
        //             }
        //             uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[0] = uIManager.player.playerWeaponSlotManager.unarmedWeapon;
        //         }
        //         else if (uIManager.leftHand02Selected)
        //         {
        //             Debug.Log("START");
        //             if (!uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[1].isUnarmed)
        //             {
        //                 uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[1]);
        //             }
        //             uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[1] = uIManager.player.playerWeaponSlotManager.unarmedWeapon;
        //             GetComponentInChildren<Image>().enabled = false;
        //         }
        //         else if (uIManager.leftHand03Selected)
        //         {
        //             if (!uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[2].isUnarmed)
        //             {
        //                 uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[2]);
        //             }
        //             uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[2] = uIManager.player.playerWeaponSlotManager.unarmedWeapon;
        //             Debug.Log("Name of icon is " + icon.name);
        //             icon.enabled = false;
        //         }
        //         //else { return; }

        //         // Have to change this for different place, I don't  want to change it right a way
        //         uIManager.player.playerInventoryManager.rightWeapon = uIManager.player.playerInventoryManager.weaponsInRightHandSlots[uIManager.player.playerInventoryManager.currentRightWeaponIndex];
        //         uIManager.player.playerInventoryManager.leftWeapon = uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[uIManager.player.playerInventoryManager.currentLeftWeaponIndex];

        //         uIManager.player.playerWeaponSlotManager.LoadWeaponOnSlot(uIManager.player.playerInventoryManager.rightWeapon, false);
        //         uIManager.player.playerWeaponSlotManager.LoadWeaponOnSlot(uIManager.player.playerInventoryManager.leftWeapon, true);

        //         uIManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(uIManager.player.playerInventoryManager);
        //         uIManager.ResetAllSelectedSlots();
        //     }
        // }

        public void UpdateThisWeaponSlot()
        {
            uIManager.itemStatsWindowUI.UpdateWeaponItemStats(weapon);
        }  
    }
}
