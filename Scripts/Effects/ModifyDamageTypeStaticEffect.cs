using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Character Effects/Static Effects/Modify Damage Type")]
    public class ModifyDamageTypeStaticEffect : StaticCharacterEffect
    {
        [Header("Damage Type Effected")]
        [SerializeField] DamageType damageType;
        [SerializeField] int modifiedValue = 0;

        // When adding the effect, we add mofied value amount to our respective damage type modifier
        public override void AddStaticEffect(CharacterManager character)
        {
            base.AddStaticEffect(character);

            switch (damageType)
            {
                case DamageType.Physical: character.characterStatsManager.physicalDamagePercentageModifier += modifiedValue;
                    break;
                case DamageType.Fire: character.characterStatsManager.fireDamagePercentageModifier += modifiedValue;
                    break;
                case DamageType.Lightning: character.characterStatsManager.lightningDamagePercentageModifier += modifiedValue;
                    break;
                case DamageType.Frost: character.characterStatsManager.frostDamagePercentageModifier += modifiedValue;
                    break;
                case DamageType.Blood: character.characterStatsManager.bloodDamagePercentageModifier += modifiedValue;
                    break;
                case DamageType.Holy: character.characterStatsManager.holyDamagePercentageModifier += modifiedValue;
                    break;
                case DamageType.Dark: character.characterStatsManager.darkDamagePercentageModifier += modifiedValue;
                    break;
                case DamageType.Magic: character.characterStatsManager.magicDamagePercentageModifier += modifiedValue;
                    break;
                default:
                    break;
            }
        }

        // When removing the effect, we substrack the amount that we added to our respective damage type modifier
        public override void RemoveStaticEffect(CharacterManager character)
        {
            base.RemoveStaticEffect(character);

            switch (damageType)
            {
                case DamageType.Physical:
                    character.characterStatsManager.physicalDamagePercentageModifier -= modifiedValue;
                    break;
                case DamageType.Fire:
                    character.characterStatsManager.fireDamagePercentageModifier -= modifiedValue;
                    break;
                case DamageType.Lightning:
                    character.characterStatsManager.lightningDamagePercentageModifier -= modifiedValue;
                    break;
                case DamageType.Frost:
                    character.characterStatsManager.frostDamagePercentageModifier -= modifiedValue;
                    break;
                case DamageType.Blood:
                    character.characterStatsManager.bloodDamagePercentageModifier -= modifiedValue;
                    break;
                case DamageType.Holy:
                    character.characterStatsManager.holyDamagePercentageModifier -= modifiedValue;
                    break;
                case DamageType.Dark:
                    character.characterStatsManager.darkDamagePercentageModifier -= modifiedValue;
                    break;
                case DamageType.Magic:
                    character.characterStatsManager.magicDamagePercentageModifier -= modifiedValue;
                    break;
                default:
                    break;
            }
        }
    }
}
