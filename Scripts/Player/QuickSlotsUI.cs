using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class QuickSlotsUI : MonoBehaviour
    {
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;
        public Image currentSpellIcon;
        public Image currentConsumableIcon;
        public TextMeshProUGUI currentConsumableAmount;
        public Image currentAmmo01Icon;
        public TextMeshProUGUI currentAmmo01Amount;
        public Image currentAmmo02Icon;
        public TextMeshProUGUI currentAmmo02Amount;

        public void UpdateWeaponQuickSlotsUI(bool isLeft, WeaponItem weapon)
        {
            if (isLeft == false)
            {
                if (weapon.itemIcon != null)
                {
                    rightWeaponIcon.sprite = weapon.itemIcon;
                    rightWeaponIcon.enabled = true;
                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
            }
            else
            {
                if (weapon.itemIcon != null)
                {
                    leftWeaponIcon.sprite = weapon.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
            }
        }

        public void UpdateCurrentSpellIcon(SpellItem spell)
        {
            if (spell.itemIcon != null)
            {
                currentSpellIcon.sprite = spell.itemIcon;
                currentSpellIcon.enabled = true;
            }
            else
            {
                currentSpellIcon.sprite = null;
                currentSpellIcon.enabled = false;
            }
        }

        public void UpdateCurrentConsumableIcon(ConsumableItem consumable)
        {
            if (consumable.itemIcon != null)
            {
                currentConsumableIcon.sprite = consumable.itemIcon;
                currentConsumableAmount.text = consumable.currentItemAmount.ToString();
                currentConsumableIcon.enabled = true;
            }
            else
            {
                currentConsumableIcon.sprite = null;
                currentConsumableAmount.text = "";
                currentConsumableIcon.enabled = false;
            }
        }

        public void UpdateAmmoQuickSlotsUI(RangedAmmoItem ammo, bool isleft)
        {
            if (isleft == true)
            {
                if (ammo != null)
                {
                    if (ammo.itemIcon != null && !ammo.isEmpty)
                    {
                        currentAmmo01Icon.sprite = ammo.itemIcon;
                        currentAmmo01Amount.text = ammo.currentAmmo.ToString();
                        currentAmmo01Icon.enabled = true;
                    }
                }
                else
                {
                    currentAmmo01Icon.sprite = null;
                    currentAmmo01Amount.text = "";
                    currentAmmo01Icon.enabled = false;
                }
            }
            else
            {
                if (ammo != null)
                {
                    if (ammo.itemIcon != null && !ammo.isEmpty)
                    {
                        currentAmmo02Icon.sprite = ammo.itemIcon;
                        currentAmmo02Amount.text = ammo.currentAmmo.ToString();
                        currentAmmo02Icon.enabled = true;
                    }
                }
                else
                {
                    currentAmmo02Icon.sprite = null;
                    currentAmmo02Amount.text = "";
                    currentAmmo02Icon.enabled = false;
                }
            }
        }
    }
}
