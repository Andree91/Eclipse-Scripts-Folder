using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Item Actions/Pyromancy Spell Action")]
    public class PyromancySpellAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting) { return; }

            //PlayerManager player = character as PlayerManager; Could also cast player like that if I want to chance this later to only apply the Player

            if (character.characterInventoryManager.currentSpell != null && character.characterInventoryManager.currentSpell.isPyroSpell)
            {
                if (character.characterStatsManager.currentFocusPoints >= character.characterInventoryManager.currentSpell.focusPointCost)
                {
                    character.characterInventoryManager.currentSpell.AttempToCastSpell(character);
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation("Shrug", true);
                }
            }
        }
    }
}
