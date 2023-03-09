using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Character Effects/Build Up/Poison Build Up")]
    public class PoisonBuildUpEffect : CharacterEffect
    {
        // The amount of poison build up given, before resistances are calculated, per game tick
        [SerializeField] float basePoisonBuildUpAmount = 7f;

        // The amount of poisonn time the character receives if the are poisoned
        [SerializeField] float poisonAmount = 100;

        // The amount of damage taken from poison per tick, if it is built up to 100%
        [SerializeField] int poisonDamagePerTick = 5;

        public override void ProcessEffect(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;

            // Poison build up after factoring character's resistances
            float finalPoisonBuildUp = 0;

            if (character.characterStatsManager.poisonResistance > 0)
            {
                // If character has 100% or more poison resistance, they immunen to poison
                if (character.characterStatsManager.poisonResistance >= 100)
                    return;
                // {
                //     finalPoisonBuildUp = 0;
                // }
                else
                {
                    float resistancePercentage = character.characterStatsManager.poisonResistance / 100;
                    finalPoisonBuildUp = basePoisonBuildUpAmount - (basePoisonBuildUpAmount * resistancePercentage);
                }
            }

            // Each tick add build up amount to the character's overall build up
            character.characterStatsManager.poisonBuildUp += finalPoisonBuildUp;

            // If character is already poisoned, remove all the poison build up effects
            if (character.characterStatsManager.isPoisoned)
            {
                character.characterEffectsManager.timedEffects.Remove(this);
            }

            // If character build up is 100% or more, poison the character
            if (character.characterStatsManager.poisonBuildUp >= 100)
            {
                character.characterStatsManager.isPoisoned = true;
                character.characterStatsManager.poisonAmount = poisonAmount;
                character.characterStatsManager.poisonBuildUp = 0;

                if (player != null)
                {
                    player.playerEffectsManager.poisonAmountBar.SetCurrentPoisonAmount(Mathf.RoundToInt(poisonAmount));
                }

                // Always Instantiate copy of original scrictable object, so original isn't ever edited
                PoisonedEffect poisonedEffect = Instantiate(WorldCharacterEffectsManager.instance.poisonedEffect);
                poisonedEffect.poisonDamage = poisonDamagePerTick;
                character.characterEffectsManager.timedEffects.Add(poisonedEffect);
                character.characterEffectsManager.timedEffects.Remove(this);
                character.characterSoundFXManager.PlaySoundFX(WorldCharacterEffectsManager.instance.poisonSFX, 0.3f);

                character.characterEffectsManager.AddTimedEffectParticle(Instantiate(WorldCharacterEffectsManager.instance.poisonFX));
            }

            character.characterEffectsManager.timedEffects.Remove(this);
        }


        // Each tick, if  character isn't poisoned, a bit of build up is removed
        // if (character.characterStatsManager.poisonBuildUp > 0 && character.characterStatsManager.poisonBuildUp < 100)
        // {
        //     character.characterStatsManager.poisonBuildUp -= 1;
        // }
    }
}