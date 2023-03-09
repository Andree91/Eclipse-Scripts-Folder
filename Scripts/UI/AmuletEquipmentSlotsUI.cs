using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class AmuletEquipmentSlotsUI : MonoBehaviour
    {
        UIManager uIManager;

        public Image icon;
        AmuletItem amulet;

        public bool amuletSlot01;
        public bool amuletSlot02;
        public bool amuletSlot03;
        public bool amuletSlot04;

        void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(AmuletItem newItem)
        {
            amulet = newItem;
            if (icon != null)
            {
                if (!amulet.isEmpty)
                {
                    icon.sprite = amulet.itemIcon;
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
            amulet = null;
            icon.sprite = null;
            icon.enabled = false;
            //gameObject.SetActive(false);
        }

        public void SelectThisSlot()
        {
            uIManager.ResetAllSelectedSlots();

            if (amuletSlot01)
            {
                uIManager.amuletSlot01Selected = true;
            }
            else if (amuletSlot02)
            {
                uIManager.amuletSlot02Selected = true;
            }
            else if (amuletSlot03)
            {
                uIManager.amuletSlot03Selected = true;
            }
            else if (amuletSlot04)
            {
                uIManager.amuletSlot04Selected = true;
            }

            uIManager.amuletSlotIsSelected = true;
            uIManager.itemStatsWindowUI.UpdateAmuletItemStats(amulet);
        }

        public void UpdateThisAmuletSlot()
        {
            uIManager.itemStatsWindowUI.UpdateAmuletItemStats(amulet);
        }
    }
}
