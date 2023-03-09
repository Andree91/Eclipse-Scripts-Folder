using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class HandEquipmentSlotUI : MonoBehaviour
    {
        UIManager uIManager;

        public Image icon;
        HandEquipment item;

        void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(HandEquipment handEquipment)
        {
            if (handEquipment != null)
            {
                item = handEquipment;
                if (icon != null)
                {
                    icon.sprite = item.itemIcon;
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
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            //gameObject.SetActive(false);
        }

        public void SelectThisSlot()
        {
            uIManager.ResetAllSelectedSlots();
            uIManager.handEquipmentSlotSelected = true;
            uIManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}
