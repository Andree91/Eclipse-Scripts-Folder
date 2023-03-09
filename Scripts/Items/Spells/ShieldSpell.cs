using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Spells/Shield Spell")]
    public class ShieldSpell : SpellItem
    {
        public float timer;

        public override void AttempToCastSpell(CharacterManager character)
        {
            if (!character.isUsingShieldSpell)
            {
                base.AttempToCastSpell(character);
                GameObject instantiateWarmUpSpellFX = Instantiate(spellWarmUpFX, character.transform);
                character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
                Debug.Log("Attemping to cas a spell... ");
                Destroy(instantiateWarmUpSpellFX, 1f);
            }
        }

        public override void SuccesfullyCastSpell(CharacterManager character)
        {
            base.SuccesfullyCastSpell(character);
            GameObject instantiateSpellFX = Instantiate(spellCastFX, character.transform);
            Debug.Log("Spell cast succesfully ");
            Destroy(instantiateSpellFX, timer);
        }
    }
}
