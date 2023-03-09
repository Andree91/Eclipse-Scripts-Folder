using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class ConsumablesInventorySlot : MonoBehaviour
    {
        public UIManager uIManager;

        public Image icon;
        public ConsumableItem item;
        public ConsumableItemDropMenu consumableItemDropMenu;
        public GameObject consumablePickUp;
        public ConsumableItem emptyItem;

        public int dropAmount;
        public TextMeshProUGUI dropAmountText;
        public Button dropAmountConfirm;

        void Awake()
        {
            if (uIManager == null)
            {
                uIManager = GetComponentInParent<UIManager>();
            }
            consumableItemDropMenu = GetComponentInChildren<ConsumableItemDropMenu>();
        }

        public void AddItem(ConsumableItem newItem)
        {
            // if (newItem != null)
            // {
            //     item = newItem;
            //     if (icon != null)
            //     {
            //         if (item.itemName != "Empty")
            //         {
            //             icon.sprite = item.itemIcon;
            //         }

            //         if (icon.sprite != null)
            //         {
            //             icon.enabled = true;
            //             gameObject.SetActive(true);
            //         }
            //     }
            // }
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
                if (uIManager.quickSlot01Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[0].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[0]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[0] = item;
                    uIManager.player.playerInventoryManager.consumablesInventory.Remove(item);
                }
                else if (uIManager.quickSlot02Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[1].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[1]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[1] = item;
                    uIManager.player.playerInventoryManager.consumablesInventory.Remove(item);
                }
                else if (uIManager.quickSlot03Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[2].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[2]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[2] = item;
                    uIManager.player.playerInventoryManager.consumablesInventory.Remove(item);
                }
                else if (uIManager.quickSlot04Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[3].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[3]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[3] = item;
                    uIManager.player.playerInventoryManager.consumablesInventory.Remove(item);
                }
                else if (uIManager.quickSlot05Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[4].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[4]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[4] = item;
                    uIManager.player.playerInventoryManager.consumablesInventory.Remove(item);
                }
                else if (uIManager.quickSlot06Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[5].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[5]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[5] = item;
                    uIManager.player.playerInventoryManager.consumablesInventory.Remove(item);
                }
                else if (uIManager.quickSlot07Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[6].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[6]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[6] = item;
                    uIManager.player.playerInventoryManager.consumablesInventory.Remove(item);
                }
                else if (uIManager.quickSlot08Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[7].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[7]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[7] = item;
                    uIManager.player.playerInventoryManager.consumablesInventory.Remove(item);
                }
                else if (uIManager.quickSlot09Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[8].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[8]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[8] = item;
                    uIManager.player.playerInventoryManager.consumablesInventory.Remove(item);
                }
                else if (uIManager.quickSlot10Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[9].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[9]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[9] = item;
                    uIManager.player.playerInventoryManager.consumablesInventory.Remove(item);
                }
                //else { return; }

                // Have to change this for different place, I don't  want to change it right a way
                uIManager.player.playerInventoryManager.currentConsumable = uIManager.player.playerInventoryManager.consumablesInQuickSlots[uIManager.player.playerInventoryManager.currentConsumableIndex];

                uIManager.equipmentWindowUI.LoadQuickSlotItemsOnEquipmentScreen(uIManager.player.playerInventoryManager);
                uIManager.quickSlotsUI.UpdateCurrentConsumableIcon(uIManager.player.playerInventoryManager.currentConsumable);
                uIManager.ResetAllSelectedSlots();
            }
        }

        public void UnEquipThisItem()
        {
            if (!uIManager.isInInventoryWindow)
            {
                if (uIManager.quickSlot01Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[0].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[0]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[0] = emptyItem; // null
                    //uIManager.equipmentWindowUI.
                    //uIManager.UptadeUI();
                }
                else if (uIManager.quickSlot02Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[1].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[1]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[1] = emptyItem;
                }
                else if (uIManager.quickSlot03Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[2].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[2]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[2] = emptyItem;
                }
                else if (uIManager.quickSlot04Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[3].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[3]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[3] = emptyItem;
                }
                else if (uIManager.quickSlot05Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[4].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[4]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[4] = emptyItem;
                }
                else if (uIManager.quickSlot06Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[5].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[5]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[5] = emptyItem;
                }
                else if (uIManager.quickSlot07Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[6].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[6]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[6] = emptyItem;
                }
                else if (uIManager.quickSlot08Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[7].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[7]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[7] = emptyItem;
                }
                else if (uIManager.quickSlot09Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[8].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[8]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[8] = emptyItem;
                }
                else if (uIManager.quickSlot10Selected)
                {
                    if (!uIManager.player.playerInventoryManager.consumablesInQuickSlots[9].isEmpty)
                    {
                        uIManager.player.playerInventoryManager.consumablesInventory.Add(uIManager.player.playerInventoryManager.consumablesInQuickSlots[9]);
                    }
                    uIManager.player.playerInventoryManager.consumablesInQuickSlots[9] = emptyItem;
                }
                //else { return; }

                // Have to change this for different place, I don't  want to change it right a way
                uIManager.player.playerInventoryManager.currentConsumable = uIManager.player.playerInventoryManager.consumablesInQuickSlots[uIManager.player.playerInventoryManager.currentConsumableIndex];

                uIManager.equipmentWindowUI.LoadQuickSlotItemsOnEquipmentScreen(uIManager.player.playerInventoryManager);
                uIManager.quickSlotsUI.UpdateCurrentConsumableIcon(uIManager.player.playerInventoryManager.currentConsumable);
                uIManager.ResetAllSelectedSlots();
            }
        }

        public void UpdateThisQuickSlot()
        {
            uIManager.itemStatsWindowUI.UpdateConsumableItemStats(item);
        }

        public void OpenConsumableItemDropMenu()
        {
            if (uIManager.isInInventoryWindow)
            {
                uIManager.inventoryConsumableItemBeingUsed = item;
                consumableItemDropMenu.consumableItemDropMenu.SetActive(true);
                consumableItemDropMenu.consumableItemDropMenu.transform.position = consumableItemDropMenu.transform.position;

                Button[] itemDropMenus = consumableItemDropMenu.consumableItemDropMenu.GetComponentsInChildren<Button>();

                if (uIManager.inventoryConsumableItemBeingUsed.canUseMultipleAtSameTime)
                {
                    //Button[] itemDropMenus = consumableItemDropMenu.consumableItemDropMenu.GetComponentsInChildren<Button>();

                    foreach (Button itemDropMenu in itemDropMenus)
                    {
                        if (itemDropMenu.name == "Use Selected")
                        {
                            itemDropMenu.interactable = true;
                        }
                    }
                }
                else
                {
                    foreach (Button itemDropMenu in itemDropMenus)
                    {
                        if (itemDropMenu.name == "Use Selected")
                        {
                            itemDropMenu.interactable = false;
                        }
                    }
                }

                if (uIManager.inventoryConsumableItemBeingUsed.isKeyItem)
                {
                    //Button[] itemDropMenus = consumableItemDropMenu.consumableItemDropMenu.GetComponentsInChildren<Button>();

                    foreach (Button itemDropMenu in itemDropMenus)
                    {
                        if (itemDropMenu.name == "Leave Button" || itemDropMenu.name == "Leave Selected Button" || itemDropMenu.name == "Discard Button")
                        {
                            itemDropMenu.interactable = false;
                        }
                    }
                }
                else
                {
                    foreach (Button itemDropMenu in itemDropMenus)
                    {
                        if (itemDropMenu.name == "Leave Button" || itemDropMenu.name == "Leave Selected Button" || itemDropMenu.name == "Discard Button")
                        {
                            itemDropMenu.interactable = true;
                        }
                    }
                }
            }
        }

        public void UseConsumableFromIventory()
        {
            //Debug.Log("Selected consumable is " + uIManager.inventoryConsumableItemBeingUsed.itemName);
            uIManager.usingThroughInventory = true;
            uIManager.player.playerInventoryManager.UseConsumableFromIventoryManager(uIManager.inventoryConsumableItemBeingUsed);
        }

        public void CloseWeaponItemDropMenu()
        {
            if (consumableItemDropMenu != null)
            {
                consumableItemDropMenu.consumableItemDropMenu.SetActive(false);
            }
        }

        public void CloseConsumableInventoryScreenWindow()
        {
            if (!uIManager.isInInventoryWindow)
            {
                uIManager.consumableInventoryWindow.SetActive(false);
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
            GameObject pickUpLive = Instantiate(consumablePickUp, uIManager.player.transform.position, Quaternion.identity);
            ConsumableItemPickUp pickUp = pickUpLive.GetComponent<ConsumableItemPickUp>();
            pickUp.item = uIManager.inventoryConsumableItemBeingUsed;
            pickUp.isLootItem = true;
            pickUp.amount = 1;
            pickUp.item.currentItemAmount -= 1;

            if (pickUp.item.currentItemAmount <= 0)
            {
                uIManager.player.playerInventoryManager.consumablesInventory.Remove(uIManager.inventoryConsumableItemBeingUsed);
            }
            
            UpdateThisQuickSlot();
        }

        public void DropMultiplyItems()
        {
            // NOT READY! TO THIS AS SOON AS YOU CAN
            GameObject pickUpLive = Instantiate(consumablePickUp, uIManager.player.transform.position, Quaternion.identity);
            ConsumableItemPickUp pickUp = pickUpLive.GetComponent<ConsumableItemPickUp>();
            pickUp.item = uIManager.inventoryConsumableItemBeingUsed;
            pickUp.isLootItem = true;
            pickUp.amount = dropAmount;

            pickUp.item.currentItemAmount -= dropAmount;

            if (pickUp.item.currentItemAmount <= 0)
            {
                uIManager.player.playerInventoryManager.consumablesInventory.Remove(uIManager.inventoryConsumableItemBeingUsed);
            }

            ResetDropAmount();
            UpdateThisQuickSlot();
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

            if (dropAmount > uIManager.inventoryConsumableItemBeingUsed.currentItemAmount)
            {
                dropAmount = uIManager.inventoryConsumableItemBeingUsed.currentItemAmount;
            }

            dropAmountText.text = dropAmount.ToString();
            dropAmountConfirm.interactable = true;
        }
    }
}
