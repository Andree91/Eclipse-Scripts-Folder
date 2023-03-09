using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class ItemStatsWindowUI : MonoBehaviour
    {
        public TextMeshProUGUI itemNameText;
        public Image itemIconImage;

        [Header("Equipment Stats Window")]
        public GameObject weaponStats;
        public GameObject rangedAmmoStats;
        public GameObject armorStats;
        public GameObject amuletStats;

        [Header("Quick Slot Item Stats Window")]
        public GameObject quickSlotItemStats;

        [Header("Weapon Stats")]
        public TextMeshProUGUI physicalDamageText;
        public TextMeshProUGUI magicDamageText;
        public TextMeshProUGUI fireDamageText;
        public TextMeshProUGUI physicalAbsorptionText;
        public TextMeshProUGUI magicAbsorptionText;
        public TextMeshProUGUI fireAbsorptionText;

        [Header("Ranged Ammo Item Stats")]
        public TextMeshProUGUI ammoAmountInInventoryText;
        public TextMeshProUGUI ammoMaxAmountInInventoryText;
        public TextMeshProUGUI ammoAmountInStoreText;
        public TextMeshProUGUI ammoMaxAmountInStoreText;
        public TextMeshProUGUI ammoItemDescriptionText;

        [Header("Armor Stats")]
        public TextMeshProUGUI armorPhysicalAbsorptionText;
        public TextMeshProUGUI armorMagicAbsorptionText;
        public TextMeshProUGUI armorFireAbsorptionText;
        public TextMeshProUGUI armorPoisonResistanceText;
        public TextMeshProUGUI armorBleedResistanceText;
        public TextMeshProUGUI armorFrostResistanceText;

        [Header("Amulet Stats")]
        public TextMeshProUGUI amuletAmountInInventoryText;
        public TextMeshProUGUI amuletMaxAmountInInventoryText;
        public TextMeshProUGUI amuletAmountInStoreText;
        public TextMeshProUGUI amuletMaxAmountInStoreText;
        public TextMeshProUGUI amuletItemDescriptionText;

        [Header("Quick Slot Item Stats")]
        public TextMeshProUGUI amountInInventoryText;
        public TextMeshProUGUI maxAmountInInventoryText;
        public TextMeshProUGUI amountInStoreText;
        public TextMeshProUGUI maxAmountInStoreText;
        public TextMeshProUGUI itemDescriptionText;

        //Update weapon item stats
        public void UpdateWeaponItemStats(WeaponItem weapon)
        {
            CloseAllStatWindows();

            if (weapon != null)
            {
                if (weapon.itemName != null)
                {
                    itemNameText.text = weapon.itemName;
                }
                else
                {
                    itemNameText.text = "Missing The Item Name";
                }

                if (weapon.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.sprite = weapon.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.sprite = null;
                }

                physicalDamageText.text = weapon.physicalDamage.ToString();
                //magicDamageText.text = weapon.magicDamage.ToString();
                fireDamageText.text = weapon.fireDamage.ToString();

                physicalAbsorptionText.text = weapon.physicalBlockingDamageAbsorption.ToString();
                magicAbsorptionText.text = weapon.magicBlockingDamageAbsorption.ToString();
                fireAbsorptionText.text = weapon.fireBlockingDamageAbsorption.ToString();

                weaponStats.SetActive(true);
            }
            else
            {
                itemNameText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.sprite = null;
                weaponStats.SetActive(false);
                
            }
        }

        //Update Armor Stats
        public void UpdateArmorItemStats(EquipmentItem armor)
        {
            CloseAllStatWindows();
            
            if (armor != null)
            {
                if (armor.itemName != null)
                {
                    itemNameText.text = armor.itemName;
                }
                else
                {
                    itemNameText.text = "Missing The Item Name";
                }

                if (armor.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.sprite = armor.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.sprite = null;
                }

                armorPhysicalAbsorptionText.text = armor.physicalDefence.ToString();
                armorMagicAbsorptionText.text = armor.magicDefence.ToString();
                armorFireAbsorptionText.text = armor.fireDefence.ToString();

                armorPoisonResistanceText.text = armor.poisonResistance.ToString();
                armorBleedResistanceText.text = armor.bleedResistance.ToString();
                armorFrostResistanceText.text = armor.frostResistance.ToString();

                armorStats.SetActive(true);
            }
            else
            {
                itemNameText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.sprite = null;
                armorStats.SetActive(false);

            }
        }

        //Update Consumable item stats
        public void UpdateConsumableItemStats(ConsumableItem consumableItem)
        {
            CloseAllStatWindows();

            if (consumableItem != null)
            {
                if (consumableItem.isEmpty)
                {
                    itemNameText.text = "";
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.sprite = null;
                    quickSlotItemStats.SetActive(false);
                    return;
                }
                if (consumableItem.itemName != null)
                {
                    itemNameText.text = consumableItem.itemName;
                }
                else
                {
                    itemNameText.text = "Missing The Item Name";
                }

                if (consumableItem.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.sprite = consumableItem.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.sprite = null;
                }

                amountInInventoryText.text = consumableItem.currentItemAmount.ToString();
                maxAmountInInventoryText.text = consumableItem.maxItemAmount.ToString();

                amountInStoreText.text = consumableItem.currentStoredItemAmount.ToString();
                maxAmountInStoreText.text = consumableItem.maxStoredAmount.ToString();

                itemDescriptionText.text = consumableItem.itemDescription;

                quickSlotItemStats.SetActive(true);
            }
            else
            {
                itemNameText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.sprite = null;
                quickSlotItemStats.SetActive(false);

            }
        }

        //Update Ranged Ammo item stats
        public void UpdateRangedAmmoItemStats(RangedAmmoItem ammoItem)
        {
            CloseAllStatWindows();

            if (ammoItem != null)
            {
                if (ammoItem.isEmpty)
                {
                    itemNameText.text = "";
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.sprite = null;
                    quickSlotItemStats.SetActive(false);
                    return;
                }
                if (ammoItem.itemName != null)
                {
                    itemNameText.text = ammoItem.itemName;
                }
                else
                {
                    itemNameText.text = "Missing The Item Name";
                }

                if (ammoItem.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.sprite = ammoItem.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.sprite = null;
                }

                ammoAmountInInventoryText.text = ammoItem.currentAmmo.ToString();
                ammoMaxAmountInInventoryText.text = ammoItem.carryLimit.ToString();

                ammoAmountInStoreText.text = ammoItem.currentStoredAmmoAmount.ToString();
                ammoMaxAmountInStoreText.text = ammoItem.maxStoredAmmoAmount.ToString();

                ammoItemDescriptionText.text = ammoItem.itemDescription;

                rangedAmmoStats.SetActive(true);
            }
            else
            {
                itemNameText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.sprite = null;
                rangedAmmoStats.SetActive(false);

            }
        }

        //Updatet Ring/Talisman item stats
        public void UpdateAmuletItemStats(AmuletItem amuletItem)
        {
            CloseAllStatWindows();

            if (amuletItem != null)
            {
                if (amuletItem.isEmpty)
                {
                    itemNameText.text = "";
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.sprite = null;
                    quickSlotItemStats.SetActive(false);
                    return;
                }
                if (amuletItem.itemName != null)
                {
                    itemNameText.text = amuletItem.itemName;
                }
                else
                {
                    itemNameText.text = "Missing The Item Name";
                }

                if (amuletItem.itemIcon != null)
                {
                    itemIconImage.gameObject.SetActive(true);
                    itemIconImage.sprite = amuletItem.itemIcon;
                }
                else
                {
                    itemIconImage.gameObject.SetActive(false);
                    itemIconImage.sprite = null;
                }

                amuletAmountInInventoryText.text = amuletItem.currentAmount.ToString();
                amuletMaxAmountInInventoryText.text = amuletItem.maxAmount.ToString();

                amuletAmountInStoreText.text = amuletItem.currentStoredAmount.ToString();
                amuletMaxAmountInStoreText.text = amuletItem.maxStoredAmount.ToString();

                amuletItemDescriptionText.text = amuletItem.itemDescription;

                amuletStats.SetActive(true);
            }
            else
            {
                itemNameText.text = "";
                itemIconImage.gameObject.SetActive(false);
                itemIconImage.sprite = null;
                amuletStats.SetActive(false);
            }
        }

        private void CloseAllStatWindows()
        {
            weaponStats.SetActive(false);
            armorStats.SetActive(false);
            quickSlotItemStats.SetActive(false);
            rangedAmmoStats.SetActive(false);
            amuletStats.SetActive(false);
        }
    }
}
