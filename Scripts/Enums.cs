using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public enum WeaponType
    {
        PyromancyCaster,
        FaithCaster,
        SpellCaster,
        StraightSword,
        GreatSword,
        ColossalSword,
        Dagger,
        ShortSword,
        Katana,
        Spear,
        Claive,
        Club,
        GreatClub,
        Axe,
        GreatAxe,
        Shield,
        SmallShield,
        GreatShield,
        Bow,
        GreatBow,
        CrossBow,
        Gun,
        Whip,
        TrickWeapon,
        Torch,
        Unarmed
    }

    public enum AmmoType
    {
        Arrow,
        GreatArrow,
        Bolt
    }

    public enum AttackType
    {
        LightAttack01,
        LightAttack02,
        HeavyAttack01,
        HeavyAttack02,
        RunningAttack,
        JumpingAttack,
        PlungingAttack,
        Slash,
        Piercing
    }

    public enum DamageType
    {
        Physical,
        Fire,
        Lightning,
        Frost,
        Blood,
        Holy,
        Dark,
        Magic
    }

    public enum BuffClass
    {
        Physical,
        Fire,
        Lightning,
        Frost,
        Blood,
        Holy,
        Dark,
        Magic,
        Poise
    }

    public enum EffectParticleType
    {
        Poison,
        StrongPoison
    }

    public enum AICombatStyle
    {
        SwordAndShield,
        HeavySword,
        HeavyShield,
        MageStaff,
        MagePyromancy,
        SpellSword,
        Archer,
        HeavyArcher,
        Thief
    }

    public enum AIAttackActionType
    {
        meleeAttackAction,
        magicAttackAction,
        rangedAttackAction
    }

    public class Enums : MonoBehaviour
    {
        
    }
}
