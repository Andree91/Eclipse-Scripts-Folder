using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class EquipmentWindowUI : MonoBehaviour
    {
        // public bool rightHandSlot01Selected;
        // public bool rightHandSlot02Selected;
        // public bool rightHandSlot03Selected;
        // public bool leftHandSlot01Selected;
        // public bool leftHandSlot02Selected;
        // public bool leftHandSlot03Selected;

        public WeaponEquipmentSlotsUI[] weaponEquipmentSlotsUI;

        public RangedAmmoEquipmentSlotsUI[] rangedAmmoEquipmentSlotsUI;

        public AmuletEquipmentSlotsUI[] amuletEquipmentSlotsUI;

        public HeadEquipmentSlotUI headEquipmentSlotUI;
        public BodyEquipmentSlotUI bodyEquipmentSlotUI;
        public HandEquipmentSlotUI handEquipmentSlotUI;
        public LegEquipmentSlotUI legEquipmentSlotUI;

        public QuickSlotsEquipmentUI[] quickSlotsEquipmentUI;

        void Awake() 
        {
            //handleEquipmentSlotsUI = GetComponentsInChildren<HandleEquipmentSlotsUI>();
        }

        public void LoadWeaponsOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            for (int i = 0; i < weaponEquipmentSlotsUI.Length; i++)
            {
                if (weaponEquipmentSlotsUI[i].rightHandSlot01)
                {
                    weaponEquipmentSlotsUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
                    if (playerInventory.weaponsInRightHandSlots[0].isUnarmed)
                    {
                        weaponEquipmentSlotsUI[i].icon.enabled = false;
                    }
                }
                else if (weaponEquipmentSlotsUI[i].rightHandSlot02)
                {
                    weaponEquipmentSlotsUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
                    if (playerInventory.weaponsInRightHandSlots[1].isUnarmed)
                    {
                        weaponEquipmentSlotsUI[i].icon.enabled = false;
                    }
                }
                else if (weaponEquipmentSlotsUI[i].rightHandSlot03)
                {
                    weaponEquipmentSlotsUI[i].AddItem(playerInventory.weaponsInRightHandSlots[2]);
                    if (playerInventory.weaponsInRightHandSlots[2].isUnarmed)
                    {
                        weaponEquipmentSlotsUI[i].icon.enabled = false;
                    }
                }
                else if (weaponEquipmentSlotsUI[i].leftHandSlot01)
                {
                    weaponEquipmentSlotsUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
                    if (playerInventory.weaponsInLeftHandSlots[0].isUnarmed)
                    {
                        weaponEquipmentSlotsUI[i].icon.enabled = false;
                    }
                }
                else if (weaponEquipmentSlotsUI[i].leftHandSlot02)
                {
                    weaponEquipmentSlotsUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
                    if (playerInventory.weaponsInLeftHandSlots[1].isUnarmed)
                    {
                        weaponEquipmentSlotsUI[i].icon.enabled = false;
                    }
                }
                else
                {
                    weaponEquipmentSlotsUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[2]);
                    if (playerInventory.weaponsInLeftHandSlots[2].isUnarmed)
                    {
                        weaponEquipmentSlotsUI[i].icon.enabled = false;
                    }
                }
            }
        }

        //     public void SelectRightHandSlot01()
        //     {
        //         rightHandSlot01Selected = true;
        //     }

        //     public void SelectRightHandSlot02()
        //     {
        //         rightHandSlot02Selected = true;
        //     }

        //     public void SelectLeftHandSlot01()
        //     {
        //         leftHandSlot01Selected = true;
        //     }

        //     public void SelectLeftHandSlot02()
        //     {
        //         leftHandSlot02Selected = true;
        //     }

        public void LoadArmorOnEquipmentScreen(PlayerInventoryManager playerInventoryManager)
        {
            if (playerInventoryManager.currentHelmetEquipment != null)
            {
                headEquipmentSlotUI.AddItem(playerInventoryManager.currentHelmetEquipment);
            }
            else
            {
                headEquipmentSlotUI.ClearItem();
            }

            if (playerInventoryManager.currentTorsoEquipment != null)
            {
                bodyEquipmentSlotUI.AddItem(playerInventoryManager.currentTorsoEquipment);
            }
            else
            {
                bodyEquipmentSlotUI.ClearItem();
            }

            if (playerInventoryManager.currentHandEquipment != null)
            {
                handEquipmentSlotUI.AddItem(playerInventoryManager.currentHandEquipment);
            }
            else
            {
                handEquipmentSlotUI.ClearItem();
            }

            if (playerInventoryManager.currentLegEquipment != null)
            {
                legEquipmentSlotUI.AddItem(playerInventoryManager.currentLegEquipment);
            }
            else
            {
                legEquipmentSlotUI.ClearItem();
            }
        }

        public void LoadQuickSlotItemsOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            // TODO Have to make system that moves and arrange availeble consumables to empty slots so if slot 1 and 2 are empty, but slot 3 isn't, then slot 3 cpnsumable is going to be first one
            for (int i = 0; i < quickSlotsEquipmentUI.Length; i++)
            {
                if (quickSlotsEquipmentUI[i].quickSlot01)
                {
                    if (playerInventory.consumablesInQuickSlots[0] != null)
                    {
                        quickSlotsEquipmentUI[i].AddItem(playerInventory.consumablesInQuickSlots[0]);
                        if (playerInventory.consumablesInQuickSlots[0].isEmpty)
                        {
                            quickSlotsEquipmentUI[i].icon.enabled = false;
                        }
                    }
                    
                }
                else if (quickSlotsEquipmentUI[i].quickSlot02)
                {
                    if (playerInventory.consumablesInQuickSlots[1] != null)
                    {
                        quickSlotsEquipmentUI[i].AddItem(playerInventory.consumablesInQuickSlots[1]);
                        if (playerInventory.consumablesInQuickSlots[1].isEmpty)
                        {
                            quickSlotsEquipmentUI[i].icon.enabled = false;
                        }
                    }
                }
                else if (quickSlotsEquipmentUI[i].quickSlot03)
                {
                    if (playerInventory.consumablesInQuickSlots[2] != null)
                    {
                        quickSlotsEquipmentUI[i].AddItem(playerInventory.consumablesInQuickSlots[2]);
                        if (playerInventory.consumablesInQuickSlots[2].isEmpty)
                        {
                            quickSlotsEquipmentUI[i].icon.enabled = false;
                        }
                    }
                }
                else if (quickSlotsEquipmentUI[i].quickSlot04)
                {
                    if (playerInventory.consumablesInQuickSlots[3] != null)
                    {
                        quickSlotsEquipmentUI[i].AddItem(playerInventory.consumablesInQuickSlots[3]);
                        if (playerInventory.consumablesInQuickSlots[3].isEmpty)
                        {
                            quickSlotsEquipmentUI[i].icon.enabled = false;
                        }
                    }
                }
                else if (quickSlotsEquipmentUI[i].quickSlot05)
                {
                    if (playerInventory.consumablesInQuickSlots[4] != null)
                    {
                        quickSlotsEquipmentUI[i].AddItem(playerInventory.consumablesInQuickSlots[4]);
                        if (playerInventory.consumablesInQuickSlots[4].isEmpty)
                        {
                            quickSlotsEquipmentUI[i].icon.enabled = false;
                        }
                    }
                }
                else if (quickSlotsEquipmentUI[i].quickSlot06)
                {
                    if (playerInventory.consumablesInQuickSlots[5] != null)
                    {
                        quickSlotsEquipmentUI[i].AddItem(playerInventory.consumablesInQuickSlots[5]);
                        if (playerInventory.consumablesInQuickSlots[5].isEmpty)
                        {
                            quickSlotsEquipmentUI[i].icon.enabled = false;
                        }
                    }
                }
                else if (quickSlotsEquipmentUI[i].quickSlot07)
                {
                    if (playerInventory.consumablesInQuickSlots[6] != null)
                    {
                        quickSlotsEquipmentUI[i].AddItem(playerInventory.consumablesInQuickSlots[6]);
                        if (playerInventory.consumablesInQuickSlots[6].isEmpty)
                        {
                            quickSlotsEquipmentUI[i].icon.enabled = false;
                        }
                    }
                }
                else if (quickSlotsEquipmentUI[i].quickSlot08)
                {
                    if (playerInventory.consumablesInQuickSlots[7] != null)
                    {
                        quickSlotsEquipmentUI[i].AddItem(playerInventory.consumablesInQuickSlots[7]);
                        if (playerInventory.consumablesInQuickSlots[7].isEmpty)
                        {
                            quickSlotsEquipmentUI[i].icon.enabled = false;
                        }
                    }
                }
                else if (quickSlotsEquipmentUI[i].quickSlot09)
                {
                    if (playerInventory.consumablesInQuickSlots[8] != null)
                    {
                        quickSlotsEquipmentUI[i].AddItem(playerInventory.consumablesInQuickSlots[8]);
                        if (playerInventory.consumablesInQuickSlots[8].isEmpty)
                        {
                            quickSlotsEquipmentUI[i].icon.enabled = false;
                        }
                    }
                }
                else if (quickSlotsEquipmentUI[i].quickSlot10)
                {
                    if (playerInventory.consumablesInQuickSlots[9] != null)
                    {
                        quickSlotsEquipmentUI[i].AddItem(playerInventory.consumablesInQuickSlots[9]);
                        if (playerInventory.consumablesInQuickSlots[9].isEmpty)
                        {
                            quickSlotsEquipmentUI[i].icon.enabled = false;
                        }
                    }
                }
            }
        }

        public void LoadRangedAmmoOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            for (int i = 0; i < rangedAmmoEquipmentSlotsUI.Length; i++)
            {
                if (rangedAmmoEquipmentSlotsUI[i].bowArrowSlot01)
                {
                    if (playerInventory.rangedAmmoItemsInAmmoSlots[0] != null)
                    {
                        rangedAmmoEquipmentSlotsUI[i].AddItem(playerInventory.rangedAmmoItemsInAmmoSlots[0]);
                        if (playerInventory.rangedAmmoItemsInAmmoSlots[0].isEmpty)
                        {
                            rangedAmmoEquipmentSlotsUI[i].icon.enabled = false;
                        }
                    }
                }
                else if (rangedAmmoEquipmentSlotsUI[i].bowArrowSlot02)
                {
                    if (playerInventory.rangedAmmoItemsInAmmoSlots[1] != null)
                    {
                        rangedAmmoEquipmentSlotsUI[i].AddItem(playerInventory.rangedAmmoItemsInAmmoSlots[1]);
                        if (playerInventory.rangedAmmoItemsInAmmoSlots[1].isEmpty)
                        {
                            rangedAmmoEquipmentSlotsUI[i].icon.enabled = false;
                        }
                    }
                }
                else if (rangedAmmoEquipmentSlotsUI[i].crossBowArrowSlot01)
                {
                    if (playerInventory.rangedAmmoItemsInAmmoSlots[2] != null)
                    {
                        rangedAmmoEquipmentSlotsUI[i].AddItem(playerInventory.rangedAmmoItemsInAmmoSlots[2]);
                        if (playerInventory.rangedAmmoItemsInAmmoSlots[2].isEmpty)
                        {
                            rangedAmmoEquipmentSlotsUI[i].icon.enabled = false;
                        }
                    }
                }
                else if (rangedAmmoEquipmentSlotsUI[i].crossBowArrowSlot02)
                {
                    if (playerInventory.rangedAmmoItemsInAmmoSlots[3] != null)
                    {
                        rangedAmmoEquipmentSlotsUI[i].AddItem(playerInventory.rangedAmmoItemsInAmmoSlots[3]);
                        if (playerInventory.rangedAmmoItemsInAmmoSlots[3].isEmpty)
                        {
                            rangedAmmoEquipmentSlotsUI[i].icon.enabled = false;
                        }
                    }
                }
            }
        }

        public void LoadAmuletItemsOnEquipmentScreen(PlayerInventoryManager playerInventory)
        {
            for (int i = 0; i < amuletEquipmentSlotsUI.Length; i++)
            {
                if (amuletEquipmentSlotsUI[i].amuletSlot01)
                {
                    if (playerInventory.currentAmuletSlot01 != null)
                    {
                        amuletEquipmentSlotsUI[i].AddItem(playerInventory.currentAmuletSlot01);
                        if (playerInventory.currentAmuletSlot01.isEmpty)
                        {
                            amuletEquipmentSlotsUI[i].icon.enabled = false;
                        }
                    }
                }
                else if (amuletEquipmentSlotsUI[i].amuletSlot02)
                {
                    if (playerInventory.currentAmuletSlot02 != null)
                    {
                        amuletEquipmentSlotsUI[i].AddItem(playerInventory.currentAmuletSlot02);
                        if (playerInventory.currentAmuletSlot02.isEmpty)
                        {
                            amuletEquipmentSlotsUI[i].icon.enabled = false;
                        }
                    }
                }
                else if (amuletEquipmentSlotsUI[i].amuletSlot03)
                {
                    if (playerInventory.currentAmuletSlot03 != null)
                    {
                        amuletEquipmentSlotsUI[i].AddItem(playerInventory.currentAmuletSlot03);
                        if (playerInventory.currentAmuletSlot03.isEmpty)
                        {
                            amuletEquipmentSlotsUI[i].icon.enabled = false;
                        }
                    }
                }
                else if (amuletEquipmentSlotsUI[i].amuletSlot04)
                {
                    if (playerInventory.currentAmuletSlot04 != null)
                    {
                        amuletEquipmentSlotsUI[i].AddItem(playerInventory.currentAmuletSlot04);
                        if (playerInventory.currentAmuletSlot04.isEmpty)
                        {
                            amuletEquipmentSlotsUI[i].icon.enabled = false;
                        }
                    }
                }
            }
        }

    }
}
