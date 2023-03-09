using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class HeadEquipmentSlotUI : MonoBehaviour
    {
        UIManager uIManager;

        public Image icon;
        HelmetEquipment item;

        void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(HelmetEquipment headEquipment)
        {
            if (headEquipment != null)
            {
                item = headEquipment;
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
            uIManager.headEquipmentSlotSelected = true;
            uIManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}
