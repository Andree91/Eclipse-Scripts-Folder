using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [System.Serializable]
    public class ClassGear
    {
        [Header("Class Name")]
        public string className;

        [Header("Weapons")]
        public WeaponItem primaryWeapon;
        public WeaponItem offHandWeapon;
        public WeaponItem secondaryWeapon;
        public WeaponItem offHandSecondaryWeapon;

        [Header("Armor")]
        public HelmetEquipment headEquipment;
        public TorsoEquipment chestEquipment;
        public HandEquipment handEquipment;
        public LegEquipment legEquipment;

        [Header("Spells")]
        public SpellItem startingSpell;
        //public SpellItem startingSpell; could give some class more spells

        [Header("Starting Gift")]
        public Item startingGift;
    }
}
