using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class LegEquipmentInventorySlot : MonoBehaviour
    {
        public UIManager uIManager;

        public Image icon;
        public LegEquipment item;
        public EquipmentItemDropMenu equipmentItemDropMenu;
        public GameObject legPickUp;

        void Awake()
        {
            if (uIManager == null)
            {
                uIManager = GetComponentInParent<UIManager>();
            }
            equipmentItemDropMenu = GetComponentInChildren<EquipmentItemDropMenu>();
        }

        public void AddItem(LegEquipment newItem)
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
            if (uIManager.legEquipmentSlotSelected)
            {
                if (uIManager.player.playerInventoryManager.currentLegEquipment != null)
                {
                    uIManager.player.playerInventoryManager.legEquipmentInventory.Add(uIManager.player.playerInventoryManager.currentLegEquipment);
                }
                uIManager.player.playerInventoryManager.currentLegEquipment = item;
                uIManager.player.playerInventoryManager.legEquipmentInventory.Remove(item);
                uIManager.player.playerEquipmentManager.EquipAllEquipmentModels();
            }
            else { return; }

            uIManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uIManager.player.playerInventoryManager);
            uIManager.ResetAllSelectedSlots();
        }

        public void UnEquipThisItem()
        {
            if (uIManager.legEquipmentSlotSelected)
            {
                if (uIManager.player.playerInventoryManager.currentLegEquipment != null)
                {
                    uIManager.player.playerInventoryManager.legEquipmentInventory.Add(uIManager.player.playerInventoryManager.currentLegEquipment);
                }
                uIManager.player.playerInventoryManager.currentLegEquipment = null;
                uIManager.player.playerEquipmentManager.EquipAllEquipmentModels();
            }
            else { return; }

            uIManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uIManager.player.playerInventoryManager);
            uIManager.ResetAllSelectedSlots();
        }

        public void UpdateThisLegSlot()
        {
            uIManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }

        public void OpenEquipmentItemDropMenu()
        {
            if (uIManager.isInInventoryWindow)
            {
                uIManager.inventoryLegItemBeingUsed = item;
                equipmentItemDropMenu.equipmwntItemDropMenu.SetActive(true);
                equipmentItemDropMenu.equipmwntItemDropMenu.transform.position = equipmentItemDropMenu.transform.position;

                if (uIManager.inventoryLegItemBeingUsed.isKeyItem)
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

        public void CloseLegEquipmentInventoryScreenWindow()
        {
            if (!uIManager.isInInventoryWindow)
            {
                uIManager.legEquipmentInventoryWindow.SetActive(false);
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
            GameObject pickUpLive = Instantiate(legPickUp, uIManager.player.transform.position, Quaternion.identity);
            LegItemPickUp pickUp = pickUpLive.GetComponent<LegItemPickUp>();
            pickUp.item = uIManager.inventoryLegItemBeingUsed;
            pickUp.isLootItem = true;
            uIManager.player.playerInventoryManager.legEquipmentInventory.Remove(uIManager.inventoryLegItemBeingUsed);
            UpdateThisLegSlot();
        }
    }
}
