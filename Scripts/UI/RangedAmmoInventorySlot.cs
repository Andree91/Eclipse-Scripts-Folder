using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class RangedAmmoInventorySlot : MonoBehaviour
    {
        public UIManager uIManager;

        public Image icon;
        public RangedAmmoItem item;
        public RangedAmmoItemDropMenu rangedAmmoItemDropMenu;
        public GameObject rangedAmmoPickUp;
        public RangedAmmoItem emptyArrowItem;
        public RangedAmmoItem emptyBoltItem;

        public int dropAmount;
        public TextMeshProUGUI dropAmountText;
        public Button dropAmountConfirm;

        void Awake()
        {
            if (uIManager == null)
            {
                uIManager = GetComponentInParent<UIManager>();
            }
            rangedAmmoItemDropMenu = GetComponentInChildren<RangedAmmoItemDropMenu>();
        }

        public void AddItem(RangedAmmoItem newItem)
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
                if (uIManager.bowArrowSlot01Selected)
                {
                    if (!uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[0].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.rangedAmmoItemsInventory.Add(uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[0]);
                    }
                    uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[0] = item;
                    uIManager.player.playerInventoryManager.rangedAmmoItemsInventory.Remove(item);
                    uIManager.player.playerInventoryManager.currentAmmo01 = item;
                    uIManager.quickSlotsUI.UpdateAmmoQuickSlotsUI(uIManager.player.playerInventoryManager.currentAmmo01, true);
                }
                else if (uIManager.bowArrowSlot02Selected)
                {
                    if (!uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[1].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.rangedAmmoItemsInventory.Add(uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[1]);
                    }
                    uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[1] = item;
                    uIManager.player.playerInventoryManager.rangedAmmoItemsInventory.Remove(item);
                    uIManager.player.playerInventoryManager.currentAmmo02 = item;
                    uIManager.quickSlotsUI.UpdateAmmoQuickSlotsUI(uIManager.player.playerInventoryManager.currentAmmo02, false);
                }
                else if (uIManager.crossBowArrowSlot01Selected)
                {
                    if (!uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[2].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.rangedAmmoItemsInventory.Add(uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[2]);
                    }
                    uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[2] = item;
                    uIManager.player.playerInventoryManager.rangedAmmoItemsInventory.Remove(item);
                    uIManager.player.playerInventoryManager.currentAmmo01 = item;
                    uIManager.quickSlotsUI.UpdateAmmoQuickSlotsUI(uIManager.player.playerInventoryManager.currentAmmo01, true);
                }
                else if (uIManager.crossBowArrowSlot02Selected)
                {
                    if (!uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[3].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.rangedAmmoItemsInventory.Add(uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[3]);
                    }
                    uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[3] = item;
                    uIManager.player.playerInventoryManager.rangedAmmoItemsInventory.Remove(item);
                    uIManager.player.playerInventoryManager.currentAmmo02 = item;
                    uIManager.quickSlotsUI.UpdateAmmoQuickSlotsUI(uIManager.player.playerInventoryManager.currentAmmo02, false);
                }
                //else { return; }

                // Have to change this for different place, I don't  want to change it right a way
                // uIManager.player.playerInventoryManager.currentAmmo01 = uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[uIManager.player.playerInventoryManager.currentAmmo01Index];
                // uIManager.player.playerInventoryManager.currentAmmo02 = uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[uIManager.player.playerInventoryManager.currentAmmo02Index];
                // Debug.Log("Current ammo 02 is " + uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[uIManager.player.playerInventoryManager.currentAmmo02Index]);

                uIManager.equipmentWindowUI.LoadRangedAmmoOnEquipmentScreen(uIManager.player.playerInventoryManager);
                uIManager.ResetAllSelectedSlots();
            }
        }

        public void UnEquipThisItem()
        {
            if (!uIManager.isInInventoryWindow)
            {
                if (uIManager.bowArrowSlot01Selected)
                {
                    if (!uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[0].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.rangedAmmoItemsInventory.Add(uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[0]);
                    }
                    uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[0] = emptyArrowItem;

                    uIManager.player.playerInventoryManager.currentAmmo01 = null;
                    uIManager.quickSlotsUI.UpdateAmmoQuickSlotsUI(uIManager.player.playerInventoryManager.currentAmmo01, true);
                }
                else if (uIManager.bowArrowSlot02Selected)
                {
                    if (!uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[1].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.rangedAmmoItemsInventory.Add(uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[1]);
                    }
                    uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[1] = emptyArrowItem;

                    uIManager.player.playerInventoryManager.currentAmmo02 = null;
                    uIManager.quickSlotsUI.UpdateAmmoQuickSlotsUI(uIManager.player.playerInventoryManager.currentAmmo02, false);
                }
                else if (uIManager.crossBowArrowSlot01Selected)
                {
                    if (!uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[2].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.rangedAmmoItemsInventory.Add(uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[2]);
                    }
                    uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[2] = emptyBoltItem;

                    uIManager.player.playerInventoryManager.currentAmmo01 = null;
                    uIManager.quickSlotsUI.UpdateAmmoQuickSlotsUI(uIManager.player.playerInventoryManager.currentAmmo01, true);
                }
                else if (uIManager.crossBowArrowSlot02Selected)
                {
                    if (!uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[3].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.rangedAmmoItemsInventory.Add(uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[3]);
                    }
                    uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[3] = emptyBoltItem;

                    uIManager.player.playerInventoryManager.currentAmmo02 = null;
                    uIManager.quickSlotsUI.UpdateAmmoQuickSlotsUI(uIManager.player.playerInventoryManager.currentAmmo02, false);
                }
                //else { return; }

                // Have to change this for different place, I don't  want to change it right a way
                // uIManager.player.playerInventoryManager.currentAmmo01 = uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[uIManager.player.playerInventoryManager.currentAmmo01Index];
                // uIManager.player.playerInventoryManager.currentAmmo02 = uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[uIManager.player.playerInventoryManager.currentAmmo02Index];
                // Debug.Log("Current ammo 02 is " + uIManager.player.playerInventoryManager.rangedAmmoItemsInAmmoSlots[uIManager.player.playerInventoryManager.currentAmmo02Index]);

                uIManager.equipmentWindowUI.LoadRangedAmmoOnEquipmentScreen(uIManager.player.playerInventoryManager);
                uIManager.ResetAllSelectedSlots();
            }
        }

        public void UpdateThisAmmoSlot()
        {
            uIManager.itemStatsWindowUI.UpdateRangedAmmoItemStats(item);
        }

        public void OpenRangedAmmoItemDropMenu()
        {
            if (uIManager.isInInventoryWindow)
            {
                uIManager.inventoryRangedAmmoItemBeingUsed = item;
                rangedAmmoItemDropMenu.rangedAmmoItemDropMenu.SetActive(true);
                rangedAmmoItemDropMenu.rangedAmmoItemDropMenu.transform.position = rangedAmmoItemDropMenu.transform.position;

                // if (uIManager.inventoryRangedAmmoItemBeingUsed.canUseMultipleAtSameTime)
                // {
                //     Button[] itemDropMenus = consumableItemDropMenu.consumableItemDropMenu.GetComponentsInChildren<Button>();

                //     foreach (Button itemDropMenu in itemDropMenus)
                //     {
                //         if (itemDropMenu.name == "Use Selected")
                //         {
                //             itemDropMenu.interactable = true;
                //         }
                //     }
                // }

                if (uIManager.inventoryRangedAmmoItemBeingUsed.isKeyItem)
                {
                    Button[] itemDropMenus = rangedAmmoItemDropMenu.rangedAmmoItemDropMenu.GetComponentsInChildren<Button>();

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

        // public void UseConsumableFromIventory()
        // {
        //     //Debug.Log("Selected consumable is " + uIManager.inventoryConsumableItemBeingUsed.itemName);
        //     uIManager.usingThroughInventory = true;
        //     uIManager.player.playerInventoryManager.UseConsumableFromIventoryManager(uIManager.inventoryConsumableItemBeingUsed);
        // }

        public void CloseRangedAmmoItemDropMenu()
        {
            if (rangedAmmoItemDropMenu != null)
            {
                rangedAmmoItemDropMenu.rangedAmmoItemDropMenu.SetActive(false);
            }
        }

        public void CloseRangedAmmoInventoryScreenWindow()
        {
            if (!uIManager.isInInventoryWindow)
            {
                uIManager.rangedAmmoInventoryWindow.SetActive(false);
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
            // NOT READY! TO THIS AS SOON AS YOU CAN
            GameObject pickUpLive = Instantiate(rangedAmmoPickUp, uIManager.player.transform.position, Quaternion.identity);
            RangedAmmoItemPickUp pickUp = pickUpLive.GetComponent<RangedAmmoItemPickUp>();
            pickUp.item = uIManager.inventoryRangedAmmoItemBeingUsed;
            pickUp.isLootItem = true;
            pickUp.amount = 1;

            pickUp.item.currentAmmo -= 1;

            if (pickUp.item.currentAmmo <= 0)
            {
                uIManager.player.playerInventoryManager.rangedAmmoItemsInventory.Remove(uIManager.inventoryRangedAmmoItemBeingUsed);
            }

            UpdateThisAmmoSlot();
        }

        public void DropMultipleItems()
        {
            // NOT READY! TO THIS AS SOON AS YOU CAN
            GameObject pickUpLive = Instantiate(rangedAmmoPickUp, uIManager.player.transform.position, Quaternion.identity);
            RangedAmmoItemPickUp pickUp = pickUpLive.GetComponent<RangedAmmoItemPickUp>();
            pickUp.item = uIManager.inventoryRangedAmmoItemBeingUsed;
            pickUp.isLootItem = true;
            pickUp.amount = dropAmount;

            pickUp.item.currentAmmo -= dropAmount;

            if (pickUp.item.currentAmmo <= 0)
            {
                uIManager.player.playerInventoryManager.rangedAmmoItemsInventory.Remove(uIManager.inventoryRangedAmmoItemBeingUsed);
            }

            ResetDropAmount();
            UpdateThisAmmoSlot();
        }

        public void ResetDropAmount()
        {
            dropAmount = 0;
            dropAmountText.text = dropAmount.ToString();

            if (dropAmount <= 0)
            {
                dropAmountConfirm.interactable = false;
            }
        }

        public void DecreaseDropAmount()
        {
            dropAmount -= 1;

            if (dropAmount <= 0)
            {
                dropAmount = 0;
                dropAmountConfirm.interactable = false;
            }

            dropAmountText.text = dropAmount.ToString();
        }

        public void IncreaseDropAmount()
        {
            dropAmount += 1;

            if (dropAmount > uIManager.inventoryRangedAmmoItemBeingUsed.currentAmmo)
            {
                dropAmount = uIManager.inventoryRangedAmmoItemBeingUsed.currentAmmo;
            }

            dropAmountText.text = dropAmount.ToString();
            dropAmountConfirm.interactable = true;
        }
    }
}
