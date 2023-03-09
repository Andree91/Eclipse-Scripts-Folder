using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class WeaponEquipmentUIRH02 : MonoBehaviour
    {
        UIManager uIManager;

        public Image icon;
        WeaponItem weapon;

        public bool rightHandSlot01;
        public bool rightHandSlot02;
        public bool rightHandSlot03;
        public bool leftHandSlot01;
        public bool leftHandSlot02;
        public bool leftHandSlot03;

        void Awake()
        {
            uIManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(WeaponItem newItem)
        {
            weapon = newItem;
            if (icon.sprite != null)
            {
                icon.enabled = true;
                gameObject.SetActive(true);
            }
        }

        public void ClearItem()
        {
            weapon = null;
            icon.sprite = null;
            icon.enabled = false;
            //gameObject.SetActive(false);
        }

        public void SelectThisSlot()
        {
            uIManager.ResetAllSelectedSlots();

            uIManager.rightHand02Selected = true;
           
            uIManager.itemStatsWindowUI.UpdateWeaponItemStats(weapon);
        }

        public void UpdateThisWeaponSlot()
        {
            if (uIManager.rightHand01Selected)
            {
                uIManager.rightHand01Selected = true;
            }
            else if (rightHandSlot02)
            {
                uIManager.rightHand02Selected = true;
            }
            else if (rightHandSlot03)
            {
                uIManager.rightHand02Selected = true;
            }
            else if (leftHandSlot01)
            {
                uIManager.leftHand01Selected = true;
            }
            else if (leftHandSlot02)
            {
                uIManager.leftHand02Selected = true;
            }
            else
            {
                uIManager.leftHand03Selected = true;
            }
            uIManager.itemStatsWindowUI.UpdateWeaponItemStats(weapon);
        }
    }
}
