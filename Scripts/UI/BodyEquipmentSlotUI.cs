using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class BodyEquipmentSlotUI : MonoBehaviour
    {
        UIManager uIManager;

        public Image icon;
        TorsoEquipment item;

        void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(TorsoEquipment torsoEquipment)
        {
            if (torsoEquipment != null)
            {
                item = torsoEquipment;
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
            uIManager.bodyEquipmentSlotSelected = true;
            uIManager.itemStatsWindowUI.UpdateArmorItemStats(item);
        }
    }
}
