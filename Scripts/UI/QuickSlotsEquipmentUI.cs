using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class QuickSlotsEquipmentUI : MonoBehaviour
    {

        UIManager uIManager;

        public Image icon;
        ConsumableItem consumableItem;

        public bool quickSlot01;
        public bool quickSlot02;
        public bool quickSlot03;
        public bool quickSlot04;
        public bool quickSlot05;
        public bool quickSlot06;
        public bool quickSlot07;
        public bool quickSlot08;
        public bool quickSlot09;
        public bool quickSlot10;

        void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(ConsumableItem newItem)
        {
            if (newItem != null)
            {
                consumableItem = newItem;
                if (icon != null)
                {
                    if (consumableItem.itemName != "Empty")
                    {
                        icon.sprite = consumableItem.itemIcon;
                    }

                    if (icon.sprite != null)
                    {
                        icon.enabled = true;
                        gameObject.SetActive(true);
                    }
                }
            }
            
        }

        public void ClearItem()
        {
            consumableItem = null;
            icon.sprite = null;
            icon.enabled = false;
            //gameObject.SetActive(false);
        }

        public void SelectThisSlot()
        {
            uIManager.ResetAllSelectedSlots();

            if (quickSlot01)
            {
                uIManager.quickSlot01Selected = true;
            }
            else if (quickSlot02)
            {
                uIManager.quickSlot02Selected = true;
            }
            else if (quickSlot03)
            {
                uIManager.quickSlot03Selected = true;
            }
            else if (quickSlot04)
            {
                uIManager.quickSlot04Selected = true;
            }
            else if (quickSlot05)
            {
                uIManager.quickSlot05Selected = true;
            }
            else if (quickSlot06)
            {
                uIManager.quickSlot06Selected = true;
            }
            else if (quickSlot07)
            {
                uIManager.quickSlot07Selected = true;
            }
            else if (quickSlot08)
            {
                uIManager.quickSlot08Selected = true;
            }
            else if (quickSlot09)
            {
                uIManager.quickSlot09Selected = true;
            }
            else if (quickSlot10)
            {
                uIManager.quickSlot10Selected = true;
            }
            uIManager.quickSlotIsSelected = true;

            uIManager.itemStatsWindowUI.UpdateConsumableItemStats(consumableItem);
        }
    }
}
