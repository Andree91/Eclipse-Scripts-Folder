using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        public List<WeaponItem> weaponsInventory;
        public List<RangedAmmoItem> rangedAmmoItemsInventory;
        public List<SpellItem> spellsInventory;
        public List<ConsumableItem> consumablesInventory;
        public List<InventoryItem> inventoryItems;
        public List<HelmetEquipment> headEquipmentInventory;
        public List<TorsoEquipment> bodyEquipmentInventory;
        public List<HandEquipment> handEquipmentInventory;
        public List<LegEquipment> legEquipmentInventory;
        public List<AmuletItem> amuletsInventory;

        PlayerManager player;

        public override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public void ChangeRightWeapon()
        {
            player.uIManager.ShowHUD();

            currentRightWeaponIndex = currentRightWeaponIndex + 1;

            if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                character.characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

            else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                character.characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

            else if (currentRightWeaponIndex == 2 && weaponsInRightHandSlots[2] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                character.characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            }
            else if (currentRightWeaponIndex == 2 && weaponsInRightHandSlots[2] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

            else if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
            {
                currentRightWeaponIndex = -1;
                rightWeapon = character.characterWeaponSlotManager.unarmedWeapon;
                character.characterWeaponSlotManager.LoadWeaponOnSlot(character.characterWeaponSlotManager.unarmedWeapon, false);
            }

            if (player.isTwoHanding)
            {
                player.inputHandler.yy_Input = true;
            }
            
        }

        public void ChangeLeftWeapon()
        {
            player.uIManager.ShowHUD();

            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

            if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                character.characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                character.characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] == null)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            else if (currentLeftWeaponIndex == 2 && weaponsInLeftHandSlots[2] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                character.characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }
            else if (currentRightWeaponIndex == 2 && weaponsInRightHandSlots[2] == null)
            {
                currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
            }

            else if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
            {
                currentLeftWeaponIndex = 0;
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                //leftWeapon = characterWeaponSlotManager.unarmedWeapon;
                //characterWeaponSlotManager.LoadWeaponOnSlot(characterWeaponSlotManager.unarmedWeapon, true);
                character.characterWeaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            }

            if (player.isTwoHanding)
            {
                player.inputHandler.yy_Input = true;
            }
        }

        public void ChangeSpellItem()
        {
            player.uIManager.ShowHUD();

            currentSpellIndex = currentSpellIndex + 1;

            if (currentSpellIndex == 0 && spellsInQuickSlots[0] != null)
            {
                currentSpell = spellsInQuickSlots[currentSpellIndex];
            }
            else if (currentSpellIndex == 0 && spellsInQuickSlots[0] == null)
            {
                currentSpellIndex = currentSpellIndex + 1;
            }

            else if (currentSpellIndex == 1 && spellsInQuickSlots[1] != null)
            {
                currentSpell = spellsInQuickSlots[currentSpellIndex];
            }
            else if (currentSpellIndex == 1 && spellsInQuickSlots[1] == null)
            {
                currentSpellIndex = currentSpellIndex + 1;
            }

            else if (currentSpellIndex == 2 && spellsInQuickSlots[2] != null)
            {
                currentSpell = spellsInQuickSlots[currentSpellIndex];
            }
            else if (currentSpellIndex == 2 && spellsInQuickSlots[2] == null)
            {
                currentSpellIndex = currentSpellIndex + 1;
            }

            else if (currentSpellIndex > spellsInQuickSlots.Length - 1)
            {
                currentSpellIndex = -1;
            }

            player.uIManager.quickSlotsUI.UpdateCurrentSpellIcon(currentSpell);
        }

        public void ChangeConsumableItem()
        {
            player.uIManager.ShowHUD();

            currentConsumableIndex = currentConsumableIndex + 1;

            if (consumablesInQuickSlots[currentConsumableIndex] != null)
            {
                if (consumablesInQuickSlots[currentConsumableIndex].itemName == "Empty")
                {
                    ChangeConsummableItemBackToFirst();
                    return;
                }
            }

            if (currentConsumableIndex == 0 && consumablesInQuickSlots[0] != null)
            {
                currentConsumable = consumablesInQuickSlots[currentConsumableIndex];
            }
            else if (currentConsumableIndex == 0 && consumablesInQuickSlots[0] == null)
            {
                currentConsumableIndex = currentConsumableIndex + 1;
            }

            else if (currentConsumableIndex == 1 && consumablesInQuickSlots[1] != null)
            {
                currentConsumable = consumablesInQuickSlots[currentConsumableIndex];
            }
            else if (currentConsumableIndex == 1 && consumablesInQuickSlots[1] == null)
            {
                currentConsumableIndex = currentConsumableIndex + 1;
            }

            else if (currentConsumableIndex == 2 && consumablesInQuickSlots[2] != null)
            {
                currentConsumable = consumablesInQuickSlots[currentConsumableIndex];
            }
            else if (currentConsumableIndex == 2 && consumablesInQuickSlots[2] == null)
            {
                currentConsumableIndex = currentConsumableIndex + 1;
            }

            else if (currentConsumableIndex == 3 && consumablesInQuickSlots[3] != null)
            {
                currentConsumable = consumablesInQuickSlots[currentConsumableIndex];
            }
            else if (currentConsumableIndex == 3 && consumablesInQuickSlots[3] == null)
            {
                currentConsumableIndex = currentConsumableIndex + 1;
            }

            else if (currentConsumableIndex == 4 && consumablesInQuickSlots[4] != null)
            {
                currentConsumable = consumablesInQuickSlots[currentConsumableIndex];
            }
            else if (currentConsumableIndex == 4 && consumablesInQuickSlots[4] == null)
            {
                currentConsumableIndex = currentConsumableIndex + 1;
            }

            else if (currentConsumableIndex == 5 && consumablesInQuickSlots[5] != null)
            {
                currentConsumable = consumablesInQuickSlots[currentConsumableIndex];
            }
            else if (currentConsumableIndex == 5 && consumablesInQuickSlots[5] == null)
            {
                currentConsumableIndex = currentConsumableIndex + 1;
            }

            else if (currentConsumableIndex == 6 && consumablesInQuickSlots[6] != null)
            {
                currentConsumable = consumablesInQuickSlots[currentConsumableIndex];
            }
            else if (currentConsumableIndex == 6 && consumablesInQuickSlots[6] == null)
            {
                currentConsumableIndex = currentConsumableIndex + 1;
            }

            else if (currentConsumableIndex == 7 && consumablesInQuickSlots[7] != null)
            {
                currentConsumable = consumablesInQuickSlots[currentConsumableIndex];
            }
            else if (currentConsumableIndex == 7 && consumablesInQuickSlots[7] == null)
            {
                currentConsumableIndex = currentConsumableIndex + 1;
            }

            else if (currentConsumableIndex == 8 && consumablesInQuickSlots[8] != null)
            {
                currentConsumable = consumablesInQuickSlots[currentConsumableIndex];
            }
            else if (currentConsumableIndex == 8 && consumablesInQuickSlots[8] == null)
            {
                currentConsumableIndex = currentConsumableIndex + 1;
            }

            else if (currentConsumableIndex == 9 && consumablesInQuickSlots[9] != null)
            {
                currentConsumable = consumablesInQuickSlots[currentConsumableIndex];
            }
            else if (currentConsumableIndex == 9 && consumablesInQuickSlots[9] == null)
            {
                currentConsumableIndex = currentConsumableIndex + 1;
            }
            
            else if (currentConsumableIndex > consumablesInQuickSlots.Length - 1)
            {
                currentConsumableIndex = -1;
            }

            player.uIManager.quickSlotsUI.UpdateCurrentConsumableIcon(currentConsumable);
        }

        public void ChangeConsummableItemBackToFirst()
        {
            player.uIManager.ShowHUD();

            if (currentConsumableIndex != 0 && consumablesInQuickSlots[0] != null)
            {
                currentConsumable = consumablesInQuickSlots[0];
                currentConsumableIndex = 0;
            }
            else if (currentConsumableIndex == 0 && consumablesInQuickSlots[0] == null)
            {
                Debug.Log("Now were are pressing and holding d pad down and is null");
                currentConsumableIndex = currentConsumableIndex + 1;
            }

            player.uIManager.quickSlotsUI.UpdateCurrentConsumableIcon(currentConsumable);
        }

        public void UseConsumableFromIventoryManager(ConsumableItem inventoryConsumableItem)
        {
            player.uIManager.ShowHUD();

            if ( inventoryConsumableItem != null)
            {
                inventoryConsumableItem.AttempToConsumeItem(player);
                Debug.Log("Name of inventory item is " + inventoryConsumableItem.itemName);
            }
            
        }

    }
}
