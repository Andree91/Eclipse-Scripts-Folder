using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class SpellItem : Item
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;
        public string spellAnimation;

        [Header("Spell Type")]
        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyroSpell;

        [Header("Spell Cost")]
        public int focusPointCost;

        [Header("Spell Description")]
        [TextArea]
        public string spellDescription;

        public virtual void AttempToCastSpell(CharacterManager character)
        {
            Debug.Log("You attempt to cast a spell");
        }

        public virtual void SuccesfullyCastSpell(CharacterManager character)
        {
            Debug.Log("You Succesfully casted a spell");
            PlayerManager player = character as PlayerManager;

            if (player != null)
            {
                player.playerStatsManager.DeductFocusPoints(focusPointCost);
            }
        }

    }
}
