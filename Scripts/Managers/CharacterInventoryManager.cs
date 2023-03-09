using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class CharacterInventoryManager : MonoBehaviour
    {
        protected CharacterManager character;

        [Header("Current Item Being Used")]
        public Item currentItemBeingUsed;

        [Header("Quick Slot Items")]
        public SpellItem currentSpell;
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public ConsumableItem currentConsumable;
        public RangedAmmoItem currentAmmo01;
        public RangedAmmoItem currentAmmo02;

        [Header("Current Equipment")]
        public HelmetEquipment currentHelmetEquipment;
        public TorsoEquipment currentTorsoEquipment;
        public HandEquipment currentHandEquipment;
        public LegEquipment currentLegEquipment;

        [Header("Current Amulet Items")]
        public AmuletItem currentAmuletSlot01;
        public AmuletItem currentAmuletSlot02;
        public AmuletItem currentAmuletSlot03;
        public AmuletItem currentAmuletSlot04;

        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[2];
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[2];
        public SpellItem[] spellsInQuickSlots = new SpellItem[2];
        public ConsumableItem[] consumablesInQuickSlots = new ConsumableItem[3];
        public RangedAmmoItem[] rangedAmmoItemsInAmmoSlots = new RangedAmmoItem[2];

        public int currentRightWeaponIndex = 0;
        public int currentLeftWeaponIndex = 0;
        public int currentSpellIndex = 0;
        public int currentConsumableIndex = 0;
        public int currentAmmo01Index = 0;
        public int currentAmmo02Index = 0;

        public virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        void Start()
        {
            character.characterWeaponSlotManager.LoadBothWeaponsOnSlot();
            LoadAmuletEffects();
        }

        // Call in save function after loading character equipment
        public virtual void LoadAmuletEffects()
        {
            if (currentAmuletSlot01 != null)
            {
                if (!currentAmuletSlot01.isEmpty)
                {
                    currentAmuletSlot01.EquipAmulet(character);
                }
            }

            if (currentAmuletSlot02 != null)
            {
                if (!currentAmuletSlot02.isEmpty)
                {
                    currentAmuletSlot02.EquipAmulet(character);
                }
            }

            if (currentAmuletSlot03 != null)
            {
                if (!currentAmuletSlot03.isEmpty)
                {
                    currentAmuletSlot03.EquipAmulet(character);
                }
            }

            if (currentAmuletSlot04 != null)
            {
                if (!currentAmuletSlot04.isEmpty)
                {
                    currentAmuletSlot04.EquipAmulet(character);
                }
            }
        }
    }
}
