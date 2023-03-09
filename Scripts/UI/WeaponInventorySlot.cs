using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        public UIManager uIManager;

        public Image icon;
        public WeaponItem item;
        public WeaponItemDropMenu weaponItemDropMenu;
        public GameObject weaponPickUp;

        void Awake() 
        {
            if (uIManager == null)
            {
                uIManager = GetComponentInParent<UIManager>();
            }
            weaponItemDropMenu = GetComponentInChildren<WeaponItemDropMenu>();
        }

        public void AddItem(WeaponItem newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void EquipThisItem()
        {
            if (!uIManager.isInInventoryWindow)
            {
                if (uIManager.rightHand01Selected)
                {
                    if (!uIManager.player.playerInventoryManager.weaponsInRightHandSlots[0].isUnarmed)
                    {
                        uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInRightHandSlots[0]);
                    }
                    uIManager.player.playerInventoryManager.weaponsInRightHandSlots[0] = item;
                    uIManager.player.playerInventoryManager.weaponsInventory.Remove(item);
                }
                else if (uIManager.rightHand02Selected)
                {
                    if (!uIManager.player.playerInventoryManager.weaponsInRightHandSlots[1].isUnarmed)
                    {
                        uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInRightHandSlots[1]);
                    }
                    uIManager.player.playerInventoryManager.weaponsInRightHandSlots[1] = item;
                    uIManager.player.playerInventoryManager.weaponsInventory.Remove(item);
                }
                else if (uIManager.rightHand03Selected)
                {
                    if (!uIManager.player.playerInventoryManager.weaponsInRightHandSlots[2].isUnarmed)
                    {
                        uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInRightHandSlots[2]);
                    }
                    uIManager.player.playerInventoryManager.weaponsInRightHandSlots[2] = item;
                    uIManager.player.playerInventoryManager.weaponsInventory.Remove(item);
                }
                else if (uIManager.leftHand01Selected)
                {
                    if (!uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[0].isUnarmed)
                    {
                        uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[0]);
                    }
                    uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[0] = item;
                    uIManager.player.playerInventoryManager.weaponsInventory.Remove(item);
                }
                else if (uIManager.leftHand02Selected)
                {
                    if (!uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[1].isUnarmed)
                    {
                        uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[1]);
                    }
                    uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[1] = item;
                    uIManager.player.playerInventoryManager.weaponsInventory.Remove(item);
                }
                else if (uIManager.leftHand03Selected)
                {
                    if (!uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[2].isUnarmed)
                    {
                        uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[2]);
                    }
                    uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[2] = item;
                    uIManager.player.playerInventoryManager.weaponsInventory.Remove(item);
                }
                //else { return; }

                // Have to change this for different place, I don't  want to change it right a way
                uIManager.player.playerInventoryManager.rightWeapon = uIManager.player.playerInventoryManager.weaponsInRightHandSlots[uIManager.player.playerInventoryManager.currentRightWeaponIndex];
                uIManager.player.playerInventoryManager.leftWeapon = uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[uIManager.player.playerInventoryManager.currentLeftWeaponIndex];

                uIManager.player.playerWeaponSlotManager.LoadWeaponOnSlot(uIManager.player.playerInventoryManager.rightWeapon, false);
                uIManager.player.playerWeaponSlotManager.LoadWeaponOnSlot(uIManager.player.playerInventoryManager.leftWeapon, true);

                uIManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(uIManager.player.playerInventoryManager);
                uIManager.ResetAllSelectedSlots();
            }
        }

        public void UnEquipThisItem()
        {
            if (!uIManager.isInInventoryWindow)
            {
                if (uIManager.rightHand01Selected)
                {
                    if (!uIManager.player.playerInventoryManager.weaponsInRightHandSlots[0].isUnarmed)
                    {
                        uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInRightHandSlots[0]);
                    }
                    uIManager.player.playerInventoryManager.weaponsInRightHandSlots[0] = uIManager.player.playerWeaponSlotManager.unarmedWeapon;
                }
                else if (uIManager.rightHand02Selected)
                {
                    if (!uIManager.player.playerInventoryManager.weaponsInRightHandSlots[1].isUnarmed)
                    {
                        uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInRightHandSlots[1]);
                    }
                    uIManager.player.playerInventoryManager.weaponsInRightHandSlots[1] = uIManager.player.playerWeaponSlotManager.unarmedWeapon;
                }
                else if (uIManager.rightHand03Selected)
                {
                    if (!uIManager.player.playerInventoryManager.weaponsInRightHandSlots[2].isUnarmed)
                    {
                        uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInRightHandSlots[2]);
                    }
                    uIManager.player.playerInventoryManager.weaponsInRightHandSlots[2] = uIManager.player.playerWeaponSlotManager.unarmedWeapon;
                }
                else if (uIManager.leftHand01Selected)
                {
                    if (!uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[0].isUnarmed)
                    {
                        uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[0]);
                    }
                    uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[0] = uIManager.player.playerWeaponSlotManager.unarmedWeapon;
                }
                else if (uIManager.leftHand02Selected)
                {
                    if (!uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[1].isUnarmed)
                    {
                        uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[1]);
                    }
                    uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[1] = uIManager.player.playerWeaponSlotManager.unarmedWeapon;
                }
                else if (uIManager.leftHand03Selected)
                {
                    if (!uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[2].isUnarmed)
                    {
                        uIManager.player.playerInventoryManager.weaponsInventory.Add(uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[2]);
                    }
                    uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[2] = uIManager.player.playerWeaponSlotManager.unarmedWeapon;
                }
                //else { return; }

                // Have to change this for different place, I don't  want to change it right a way
                uIManager.player.playerInventoryManager.rightWeapon = uIManager.player.playerInventoryManager.weaponsInRightHandSlots[uIManager.player.playerInventoryManager.currentRightWeaponIndex];
                uIManager.player.playerInventoryManager.leftWeapon = uIManager.player.playerInventoryManager.weaponsInLeftHandSlots[uIManager.player.playerInventoryManager.currentLeftWeaponIndex];

                uIManager.player.playerWeaponSlotManager.LoadWeaponOnSlot(uIManager.player.playerInventoryManager.rightWeapon, false);
                uIManager.player.playerWeaponSlotManager.LoadWeaponOnSlot(uIManager.player.playerInventoryManager.leftWeapon, true);

                uIManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(uIManager.player.playerInventoryManager);
                uIManager.ResetAllSelectedSlots();
            }
        }

        public void UpdateThisWeaponSlot()
        {
            //uIManager.ResetAllSelectedSlots();
            uIManager.itemStatsWindowUI.UpdateWeaponItemStats(item);
        }

        public void OpenWeaponItemDropMenu()
        {
            if (uIManager.isInInventoryWindow)
            {
                uIManager.inventoryWeaponItemBeingUsed = item;
                weaponItemDropMenu.weaponItemDropMenu.SetActive(true);
                weaponItemDropMenu.weaponItemDropMenu.transform.position = weaponItemDropMenu.transform.position;

                if (uIManager.inventoryWeaponItemBeingUsed.isKeyItem)
                {
                    Button[] itemDropMenus = weaponItemDropMenu.weaponItemDropMenu.GetComponentsInChildren<Button>();

                    foreach (Button itemDropMenu in itemDropMenus)
                    {
                        if (itemDropMenu.name == "Leave Button" || itemDropMenu.name == "Discard Button")
                        {
                            itemDropMenu.interactable = false;
                        }
                    }
                }
            }
        }

        public void CloseWeaponItemDropMenu()
        {
            if (weaponItemDropMenu != null)
            {
                weaponItemDropMenu.weaponItemDropMenu.SetActive(false);
            }
        }

        public void CloseWeaponInventoryScreenWindow()
        {
            if (!uIManager.isInInventoryWindow)
            {
                uIManager.weaponInventoryWindow.SetActive(false);
            }
        }

        public void OpenEquipmentSreennWindow()
        {
            if (!uIManager.isInInventoryWindow)
            {
                uIManager.equipmentScreenWindow.SetActive(true);
            }
        }

        public void DropItem()
        {
            GameObject pickUpLive = Instantiate(weaponPickUp, uIManager.player.transform.position, Quaternion.identity);
            WeaponPickUp pickUp = pickUpLive.GetComponent<WeaponPickUp>();
            pickUp.weapon = uIManager.inventoryWeaponItemBeingUsed;
            pickUp.isLootItem = true;
            uIManager.player.playerInventoryManager.weaponsInventory.Remove(uIManager.inventoryWeaponItemBeingUsed);
            UpdateThisWeaponSlot();
        }

    }
}