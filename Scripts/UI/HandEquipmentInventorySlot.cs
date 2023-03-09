using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class HandEquipmentInventorySlot : MonoBehaviour
    {
        public UIManager uIManager;

        public Image icon;
        public HandEquipment item;
        public EquipmentItemDropMenu equipmentItemDropMenu;
        public GameObject handPickUp;

        void Awake()
        {
            if (uIManager == null)
            {
                uIManager = GetComponentInParent<UIManager>();
            }
            equipmentItemDropMenu = GetComponentInChildren<EquipmentItemDropMenu>();
        }

        public void AddItem(HandEquipment newItem)
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
            if (uIManager.handEquipmentSlotSelected)
            {
                if (uIManager.player.playerInventoryManager.currentHandEquipment != null)
                {
                    uIManager.player.playerInventoryManager.handEquipmentInventory.Add(uIManager.player.playerInventoryManager.currentHandEquipment);
                }
                uIManager.player.playerInventoryManager.currentHandEquipment = item;
                uIManager.player.playerInventoryManager.handEquipmentInventory.Remove(item);
                uIManager.player.playerEquipmentManager.EquipAllEquipmentModels();
            }
            else { return; }

            uIManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uIManager.player.playerInventoryManager);
            uIManager.ResetAllSelectedSlots();
        }

        public void UnEquipThisItem()
        {
            if (uIManager.handEquipmentSlotSelected)
            {
                if (uIManager.player.playerInventoryManager.currentHandEquipment != null)
                {
                    uIManager.player.playerInventoryManager.handEquipmentInventory.Add(uIManager.player.playerInventoryManager.currentHandEquipment);
                }
                uIManager.player.playerInventoryManager.currentHandEquipment = null;
                uIManager.player.playerEquipmentManager.EquipAllEquipmentModels();
            }
            else { return; }

            uIManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uIManager.player.playerInventoryManager);
            uIManager.ResetAllSelectedSlots();
        }

        public void UpdateThisHandSlot()
        {
            uIManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }

        public void OpenEquipmentItemDropMenu()
        {
            if (uIManager.isInInventoryWindow)
            {
                uIManager.inventoryHandItemBeingUsed = item;
                equipmentItemDropMenu.equipmwntItemDropMenu.SetActive(true);
                equipmentItemDropMenu.equipmwntItemDropMenu.transform.position = equipmentItemDropMenu.transform.position;

                if (uIManager.inventoryHandItemBeingUsed.isKeyItem)
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

        public void CloseHandEquipmentInventoryScreenWindow()
        {
            if (!uIManager.isInInventoryWindow)
            {
                uIManager.handEquipmentInventoryWindow.SetActive(false);
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
            GameObject pickUpLive = Instantiate(handPickUp, uIManager.player.transform.position, Quaternion.identity);
            HandItemPickUp pickUp = pickUpLive.GetComponent<HandItemPickUp>();
            pickUp.item = uIManager.inventoryHandItemBeingUsed;
            pickUp.isLootItem = true;
            uIManager.player.playerInventoryManager.handEquipmentInventory.Remove(uIManager.inventoryHandItemBeingUsed);
            UpdateThisHandSlot();
        }
    }
}
