using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class LegEquipmentSlotUI : MonoBehaviour
    {
        UIManager uIManager;

        public Image icon;
        LegEquipment item;

        void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(LegEquipment legEquipment)
        {
            if (legEquipment != null)
            {
                item = legEquipment;
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
            uIManager.legEquipmentSlotSelected = true;
            uIManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}
