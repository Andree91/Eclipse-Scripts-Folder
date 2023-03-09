using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class HeadEquipmentInventorySlot : MonoBehaviour
    {
        public UIManager uIManager;

        public Image icon;
        public HelmetEquipment item;
        public EquipmentItemDropMenu equipmentItemDropMenu;
        public GameObject helmetPickUp;

        void Awake()
        {
            if (uIManager == null)
            {
                uIManager = GetComponentInParent<UIManager>();
            }
            equipmentItemDropMenu = GetComponentInChildren<EquipmentItemDropMenu>();
        }

        public void AddItem(HelmetEquipment newItem)
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
            if (uIManager.headEquipmentSlotSelected)
            {
                if (uIManager.player.playerInventoryManager.currentHelmetEquipment != null)
                {
                    uIManager.player.playerInventoryManager.headEquipmentInventory.Add(uIManager.player.playerInventoryManager.currentHelmetEquipment);
                }
                uIManager.player.playerInventoryManager.currentHelmetEquipment = item;
                uIManager.player.playerInventoryManager.headEquipmentInventory.Remove(item);
                uIManager.player.playerEquipmentManager.EquipAllEquipmentModels();
            }
            else { return; }

            uIManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uIManager.player.playerInventoryManager);
            uIManager.ResetAllSelectedSlots();
        }

        public void UnEquipThisItem()
        {
            if (uIManager.headEquipmentSlotSelected)
            {
                if (uIManager.player.playerInventoryManager.currentHelmetEquipment != null)
                {
                    uIManager.player.playerInventoryManager.headEquipmentInventory.Add(uIManager.player.playerInventoryManager.currentHelmetEquipment);
                }
                uIManager.player.playerInventoryManager.currentHelmetEquipment = null;
                uIManager.player.playerEquipmentManager.EquipAllEquipmentModels();
            }
            else { return; }

            uIManager.equipmentWindowUI.LoadArmorOnEquipmentScreen(uIManager.player.playerInventoryManager);
            uIManager.ResetAllSelectedSlots();
        }

        public void UpdateThisHeadSlot()
        {
            uIManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }

        public void OpenEquipmentItemDropMenu()
        {
            if (uIManager.isInInventoryWindow)
            {
                uIManager.inventoryHelmetItemBeingUsed = item;
                equipmentItemDropMenu.equipmwntItemDropMenu.SetActive(true);
                equipmentItemDropMenu.equipmwntItemDropMenu.transform.position = equipmentItemDropMenu.transform.position;

                if (uIManager.inventoryHelmetItemBeingUsed.isKeyItem)
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

        public void CloseHeadEquipmentInventoryScreenWindow()
        {
            if (!uIManager.isInInventoryWindow)
            {
                uIManager.headEquipmentInventoryWindow.SetActive(false);
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
            GameObject pickUpLive = Instantiate(helmetPickUp, uIManager.player.transform.position, Quaternion.identity);
            HelmetItemPickUp pickUp = pickUpLive.GetComponent<HelmetItemPickUp>();
            pickUp.item = uIManager.inventoryHelmetItemBeingUsed;
            pickUp.isLootItem = true;
            uIManager.player.playerInventoryManager.headEquipmentInventory.Remove(uIManager.inventoryHelmetItemBeingUsed);
            UpdateThisHeadSlot();
        }
    }
}
