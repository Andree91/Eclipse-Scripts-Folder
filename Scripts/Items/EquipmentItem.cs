using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class EquipmentItem : Item
    {
        [Header("Defence Bonus")]
        public float physicalDefence;
        public float magicDefence;
        public float lightningDefence;
        public float fireDefence;
        public float holyDefence;
        //And many more

        [Header("Resistances")]
        public float poisonResistance;
        public float bleedResistance;
        public float frostResistance;
        public float curseResistance;
        //And many more
    }
}
