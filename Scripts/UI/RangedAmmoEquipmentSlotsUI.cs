using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class RangedAmmoEquipmentSlotsUI : MonoBehaviour
    {
        UIManager uIManager;

        public Image icon;
        RangedAmmoItem ammo;

        public bool bowArrowSlot01;
        public bool bowArrowSlot02;
        public bool crossBowArrowSlot01;
        public bool crossBowArrowSlot02;

        void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(RangedAmmoItem newItem)
        {
            ammo = newItem;
            if (icon != null)
            {
                if (!ammo.isEmpty)
                {
                    icon.sprite = ammo.itemIcon;
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
            ammo = null;
            icon.sprite = null;
            icon.enabled = false;
            //gameObject.SetActive(false);
        }

        public void SelectThisSlot()
        {
            uIManager.ResetAllSelectedSlots();

            if (bowArrowSlot01)
            {
                uIManager.bowArrowSlot01Selected = true;
                //uIManager.rightHand02Selected = false;
            }
            else if (bowArrowSlot02)
            {
                uIManager.bowArrowSlot02Selected = true;
            }
            else if (crossBowArrowSlot01)
            {
                uIManager.crossBowArrowSlot01Selected = true;
            }
            else if (crossBowArrowSlot02)
            {
                uIManager.crossBowArrowSlot02Selected = true;
            }
            
            uIManager.arrowSlotIsSelected = true;
            uIManager.itemStatsWindowUI.UpdateRangedAmmoItemStats(ammo);
        }

        public void UpdateThisAmmoSlot()
        {
            uIManager.itemStatsWindowUI.UpdateRangedAmmoItemStats(ammo);
        }
    }
}
