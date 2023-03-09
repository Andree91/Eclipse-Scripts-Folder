using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttempToCastSpell(CharacterManager character)
        {
            base.AttempToCastSpell(character);
            GameObject instantiateWarmUpSpellFX = Instantiate(spellWarmUpFX, character.transform);
            character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
            Debug.Log("Attemping to cas a spell... ");
            Destroy(instantiateWarmUpSpellFX, 1f);
        }

        public override void SuccesfullyCastSpell(CharacterManager character)
        {
            base.SuccesfullyCastSpell(character);
            GameObject instantiateSpellFX = Instantiate(spellCastFX, character.transform);
            character.characterStatsManager.HealCharacter(healAmount);
            Debug.Log("Spell cast succesfully ");
            Destroy(instantiateSpellFX, 3f);
        }
    }
}
