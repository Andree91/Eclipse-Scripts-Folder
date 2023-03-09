using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Items/Amulet")]
    public class AmuletItem : Item
    {
       [SerializeField] StaticCharacterEffect staticEffect;
       private StaticCharacterEffect effectClone;

        // For showing in UI
       [Header("Item Effect Description")]
       [TextArea] public string itemEffectDescription;

        [Header("Amulet Capacity")]
        public int maxAmount = 99;
        public int currentAmount = 0;
        public int maxStoredAmount = 99;
        public int currentStoredAmount;

        public bool isEmpty;

       // Called when equipping the amulet, adds the static effect to the character wearing the amulet
       public void EquipAmulet(CharacterManager character)
       {
            // Add static effect on character
            // We creare a clone so basic sriptable object isn't affected if we change any of its variables
            effectClone = Instantiate(staticEffect);

            character.characterEffectsManager.AddStaticEffect(effectClone);
       }

        // Called when UNequipping the amulet, removes the static effect from character who was wearing the amulet
        public void UnEquipAmulet(CharacterManager character)
       {
            if (staticEffect != null)
            {
                character.characterEffectsManager.RemoveStaticEffect(staticEffect.effectID);
            }
       }
    }
}
