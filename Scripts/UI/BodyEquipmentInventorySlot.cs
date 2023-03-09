using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class BodyEquipmentInventorySlot : MonoBehaviour
    {
        public UIManager uIManager;

        public Image icon;
        public TorsoEquipment item;
        public EquipmentItemDropMenu equipmentItemDropMenu;
        public GameObject bodyPickUp;


        void Awake()
        {
            if (uIManager == null)
            {
                uIManager = GetComponentInParent<UIManager>();
            }
            equipmentItemDropMenu = GetComponentInChildren<EquipmentItemDropMenu>();
        }

        public void AddItem(TorsoEquipment newItem)
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
            if (uIManager.bodyEquipmentSlotSelected)
            {
                if (uIManager.player.playerInventoryManager.currentTorsoEquipment != null)
                {
                    uIManager.player.playerInventoryManager.bodyEquipmentInventory.Add(uIManager.player.playerInventoryManager.currentTorsoEquipment);
                }
                uIManager.player.playerInventoryManager.currentTorsoEquipment = item;
                uIManager.player.playerInventoryManager.bodyEquipmentInventory.Remove(item);
                uIManager.player.playerEquipmentManager.EquipAllEquipmentModels();
            }
            else { return; }

            uIManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uIManager.player.playerInventoryManager);
            uIManager.ResetAllSelectedSlots();
        }

        public void UnEquipThisItem()
        {
            if (uIManager.bodyEquipmentSlotSelected)
            {
                if (uIManager.player.playerInventoryManager.currentTorsoEquipment != null)
                {
                    uIManager.player.playerInventoryManager.bodyEquipmentInventory.Add(uIManager.player.playerInventoryManager.currentTorsoEquipment);
                }
                uIManager.player.playerInventoryManager.currentTorsoEquipment = null;
                uIManager.player.playerEquipmentManager.EquipAllEquipmentModels();
            }
            else { return; }

            uIManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uIManager.player.playerInventoryManager);
            uIManager.ResetAllSelectedSlots();
        }

        public void UpdateThisBodySlot()
        {
            uIManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }

        public void OpenEquipmentItemDropMenu()
        {
            if (uIManager.isInInventoryWindow)
            {
                uIManager.inventoryBodyItemBeingUsed = item;
                equipmentItemDropMenu.equipmwntItemDropMenu.SetActive(true);
                equipmentItemDropMenu.equipmwntItemDropMenu.transform.position = equipmentItemDropMenu.transform.position;

                if (uIManager.inventoryBodyItemBeingUsed.isKeyItem)
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

        public void CloseBodyEquipmentInventoryScreenWindow()
        {
            if (!uIManager.isInInventoryWindow)
            {
                uIManager.bodyEquipmentInventoryWindow.SetActive(false);
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
            GameObject pickUpLive = Instantiate(bodyPickUp, uIManager.player.transform.position, Quaternion.identity);
            BodyItemPickUp pickUp = pickUpLive.GetComponent<BodyItemPickUp>();
            pickUp.item = uIManager.inventoryBodyItemBeingUsed;
            pickUp.isLootItem = true;
            uIManager.player.playerInventoryManager.bodyEquipmentInventory.Remove(uIManager.inventoryBodyItemBeingUsed);
            UpdateThisBodySlot();
        }
    }
}
