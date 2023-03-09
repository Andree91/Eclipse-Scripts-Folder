using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Item Actions/Critical Attack Action")]
    public class CriticalAttackAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting) { return; }

            character.characterAnimatorManager.EraseHandIKForWeapon();

            character.characterCombatManager.AttemptBackStabOrRiposte();
        }
    }
}
