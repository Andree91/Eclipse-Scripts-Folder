using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Animator Replacer")]
        public AnimatorOverrideController weaponController;
        //public string offHandIdleAnimation = "Left_Arm_Idle_01";

        [Header("Weapon Type")]
        public WeaponType weaponType;

        // [Header("Weapon Buff Type")]
        // public DamageType weaponBuffType;


        [Header("Damage")]
        public int physicalDamage = 25;
        public int fireDamage;
        public int lightingDamage;
        public int holyDamage;
        public int frostDamage;

        [Header("Damage Modifiers")]
        public float lightAttack01DamageModifier;
        public float lightAttack02DamageModifier;
        public float heavyAttack01DamageModifier;
        public float heavyAttack02DamageModifier;
        public float runningAttackDamageModifier;
        public float jumpingAttackDamageModifier;
        public float plungingAttackDamageModifier;
        public int criticalDamageMuiltiplier = 4;
        public float guardBreakModifier = 1;

        [Header("Buffs")]
        public bool canBeBuffed;
        public bool isBuffed;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Blocking Absorption")]
        public float physicalBlockingDamageAbsorption;
        public float fireBlockingDamageAbsorption;
        public float magicBlockingDamageAbsorption;
        public float lightningBlockingDamageAbsorption;
        public float holyBlockingDamageAbsorption;

        [Header("Stamina Cost")]
        public int baseStaminaCost;
        public float lightAttackStaminaMultiplier;
        public float heavyAttackStaminaMultiplier;
        public float jumpingAttackStaminaMultiplier;

        [Header("Stability")]
        public float stability = 67;

        [Header("One Handed Item Actions")]
        public ItemAction hold_RB_action;
        public ItemAction tap_RB_action;
        public ItemAction hold_RT_action;
        public ItemAction tap_RT_action;
        public ItemAction hold_LB_action;
        public ItemAction tap_LB_action;
        public ItemAction hold_LT_action;
        public ItemAction tap_LT_action;
        public ItemAction release_RB_action;
        public ItemAction release_RT_action;

        [Header("Two Handed Item Actions")]
        public ItemAction th_hold_RB_action;
        public ItemAction th_tap_RB_action;
        public ItemAction th_hold_RT_action;
        public ItemAction th_tap_RT_action;
        public ItemAction th_hold_LB_action;
        public ItemAction th_tap_LB_action;
        public ItemAction th_hold_LT_action;
        public ItemAction th_tap_LT_action;
        public ItemAction th_release_RB_action;
        public ItemAction th_release_RT_action;

        [Header("Sound FX")]
        public AudioClip[] weaponWhooshes;
        
    }
}
