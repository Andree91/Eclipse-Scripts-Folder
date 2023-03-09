using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Poisoned Effect")]
    public class PoisonedEffect : CharacterEffect
    {
        public int poisonDamage = 1;

        public override void ProcessEffect(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;
            if (character.characterStatsManager.isPoisoned)
            {
                if (character.characterStatsManager.poisonAmount > 0)
                {
                    character.characterStatsManager.poisonAmount -= character.characterEffectsManager.effectDecayAmount;
                    // Damage Character

                    if (player != null)
                    {
                        player.playerEffectsManager.poisonAmountBar.SetCurrentPoisonAmount(Mathf.RoundToInt(character.characterStatsManager.poisonAmount));
                    }
                }
                else
                {
                    character.characterStatsManager.isPoisoned = false;
                    character.characterStatsManager.poisonAmount = 0;

                    if (player != null)
                    {
                        player.playerEffectsManager.poisonAmountBar.SetCurrentPoisonAmount(0);
                    }
                }
            }
            else
            {
                character.characterEffectsManager.timedEffects.Remove(this);
                character.characterEffectsManager.RemoveTimedEffectParticle(EffectParticleType.Poison);
            }
        }
    }
}