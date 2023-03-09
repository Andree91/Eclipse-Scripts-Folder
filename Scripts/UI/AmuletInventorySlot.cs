using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class AmuletInventorySlot : MonoBehaviour
    {
        public UIManager uIManager;

        public Image icon;
        public AmuletItem item;
        public EquipmentItemDropMenu equipmentItemDropMenu;
        public GameObject amuletPickUp;
        public AmuletItem emptyAmuletItem;

        void Awake()
        {
            if (uIManager == null)
            {
                uIManager = GetComponentInParent<UIManager>();
            }
            equipmentItemDropMenu = GetComponentInChildren<EquipmentItemDropMenu>();
        }

        public void AddItem(AmuletItem newItem)
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
                if (uIManager.amuletSlot01Selected)
                {
                    if (!uIManager.player.playerInventoryManager.currentAmuletSlot01.isEmpty)
                    {
                        uIManager.player.playerInventoryManager.amuletsInventory.Add(uIManager.player.playerInventoryManager.currentAmuletSlot01);
                    }
                    uIManager.player.playerInventoryManager.currentAmuletSlot01 = item;
                    uIManager.player.playerInventoryManager.amuletsInventory.Remove(item);
                    uIManager.player.playerInventoryManager.currentAmuletSlot01.EquipAmulet(uIManager.player);
                }
                else if (uIManager.amuletSlot02Selected)
                {
                    if (!uIManager.player.playerInventoryManager.currentAmuletSlot02.isEmpty)
                    {
                        uIManager.player.playerInventoryManager.amuletsInventory.Add(uIManager.player.playerInventoryManager.currentAmuletSlot02);
                    }
                    uIManager.player.playerInventoryManager.currentAmuletSlot02 = item;
                    uIManager.player.playerInventoryManager.amuletsInventory.Remove(item);
                    uIManager.player.playerInventoryManager.currentAmuletSlot02.EquipAmulet(uIManager.player);
                }
                else if (uIManager.amuletSlot03Selected)
                {
                    if (!uIManager.player.playerInventoryManager.currentAmuletSlot03.isEmpty)
                    {
                        uIManager.player.playerInventoryManager.amuletsInventory.Add(uIManager.player.playerInventoryManager.currentAmuletSlot03);
                    }
                    uIManager.player.playerInventoryManager.currentAmuletSlot03 = item;
                    uIManager.player.playerInventoryManager.amuletsInventory.Remove(item);
                    uIManager.player.playerInventoryManager.currentAmuletSlot03.EquipAmulet(uIManager.player);
                }
                else if (uIManager.amuletSlot04Selected)
                {
                    if (!uIManager.player.playerInventoryManager.currentAmuletSlot04.isEmpty)
                    {
                        uIManager.player.playerInventoryManager.amuletsInventory.Add(uIManager.player.playerInventoryManager.currentAmuletSlot04);
                    }
                    uIManager.player.playerInventoryManager.currentAmuletSlot04 = item;
                    uIManager.player.playerInventoryManager.amuletsInventory.Remove(item);
                    uIManager.player.playerInventoryManager.currentAmuletSlot04.EquipAmulet(uIManager.player);
                }
                //else { return; }

                // Have to change this for different place, I don't  want to change it right a way
                // uIManager.player.playerInventoryManager.currentAmmo01 = uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[uIManager.player.playerInventoryManager.currentAmmo01Index];
                // uIManager.player.playerInventoryManager.currentAmmo02 = uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[uIManager.player.playerInventoryManager.currentAmmo02Index];
                // Debug.Log("Current ammo 02 is " + uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[uIManager.player.playerInventoryManager.currentAmmo02Index]);

                uIManager.equipmentWindowUI.LoadAmuletItemsOnEquipmentScreen(uIManager.player.playerInventoryManager);
                uIManager.ResetAllSelectedSlots();
            }
        }

        public void UnEquipThisItem()
        {
            if (!uIManager.isInInventoryWindow)
            {
                if (uIManager.amuletSlot01Selected)
                {
                    if (!uIManager.player.playerInventoryManager.currentAmuletSlot01.isEmpty)
                    {
                        uIManager.player.playerInventoryManager.amuletsInventory.Add(uIManager.player.playerInventoryManager.currentAmuletSlot01);
                        uIManager.player.playerInventoryManager.currentAmuletSlot01.UnEquipAmulet(uIManager.player);
                    }
                    uIManager.player.playerInventoryManager.currentAmuletSlot01 = emptyAmuletItem;
                }
                else if (uIManager.amuletSlot02Selected)
                {
                    if (!uIManager.player.playerInventoryManager.currentAmuletSlot02.isEmpty)
                    {
                        uIManager.player.playerInventoryManager.amuletsInventory.Add(uIManager.player.playerInventoryManager.currentAmuletSlot02);
                        uIManager.player.playerInventoryManager.currentAmuletSlot02.UnEquipAmulet(uIManager.player);
                    }
                    uIManager.player.playerInventoryManager.currentAmuletSlot02 = emptyAmuletItem;
                }
                else if (uIManager.amuletSlot03Selected)
                {
                    if (!uIManager.player.playerInventoryManager.currentAmuletSlot03.isEmpty)
                    {
                        uIManager.player.playerInventoryManager.amuletsInventory.Add(uIManager.player.playerInventoryManager.currentAmuletSlot03);
                        uIManager.player.playerInventoryManager.currentAmuletSlot03.UnEquipAmulet(uIManager.player);
                    }
                    uIManager.player.playerInventoryManager.currentAmuletSlot03 = emptyAmuletItem;
                }
                else if (uIManager.amuletSlot04Selected)
                {
                    if (!uIManager.player.playerInventoryManager.currentAmuletSlot04.isEmpty)
                    {
                        uIManager.player.playerInventoryManager.amuletsInventory.Add(uIManager.player.playerInventoryManager.currentAmuletSlot04);
                        uIManager.player.playerInventoryManager.currentAmuletSlot04.UnEquipAmulet(uIManager.player);
                    }
                    uIManager.player.playerInventoryManager.currentAmuletSlot04 = emptyAmuletItem;
                }
                //else { return; }

                // Have to change this for different place, I don't  want to change it right a way
                // uIManager.player.playerInventoryManager.currentAmmo01 = uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[uIManager.player.playerInventoryManager.currentAmmo01Index];
                // uIManager.player.playerInventoryManager.currentAmmo02 = uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[uIManager.player.playerInventoryManager.currentAmmo02Index];
                // Debug.Log("Current ammo 02 is " + uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[uIManager.player.playerInventoryManager.currentAmmo02Index]);

                uIManager.equipmentWindowUI.LoadAmuletItemsOnEquipmentScreen(uIManager.player.playerInventoryManager);
                uIManager.ResetAllSelectedSlots();
            }
        }

        public void UpdateThisAmuletSlot()
        {
            uIManager.itemStatsWindowUI.UpdateAmuletItemStats(item);
        }

        public void OpenEquipmentItemDropMenu()
        {
            if (uIManager.isInInventoryWindow)
            {
                uIManager.inventoryAmuletItemBeingUsed = item;
                equipmentItemDropMenu.equipmwntItemDropMenu.SetActive(true);
                equipmentItemDropMenu.equipmwntItemDropMenu.transform.position = equipmentItemDropMenu.transform.position;

                if (uIManager.inventoryAmuletItemBeingUsed.isKeyItem)
                {
                    Button[] itemDropMenus = equipmentItemDropMenu.equipmwntItemDropMenu.GetComponentsInChildren<Button>();

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

        public void CloseEquipmentItemDropMenu()
        {
            if (equipmentItemDropMenu != null)
            {
                equipmentItemDropMenu.equipmwntItemDropMenu.SetActive(false);
            }
        }

        public void CloseAmuletInventoryScreenWindow()
        {
            if (!uIManager.isInInventoryWindow)
            {
                uIManager.amuletInventoryWindow.SetActive(false);
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
            GameObject pickUpLive = Instantiate(amuletPickUp, uIManager.player.transform.position, Quaternion.identity);
            AmuletItemPickUp pickUp = pickUpLive.GetComponent<AmuletItemPickUp>();
            pickUp.item = uIManager.inventoryAmuletItemBeingUsed;
            pickUp.isLootItem = true;
            uIManager.player.playerInventoryManager.amuletsInventory.Remove(uIManager.inventoryAmuletItemBeingUsed);
            UpdateThisAmuletSlot();
        }
    }
}

