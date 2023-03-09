using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class StaticCharacterEffect : ScriptableObject
    {
        public int effectID;
        public Sprite effectIcon;

        // Static effect is added to character while wearing an item, and effect is removed when unequiping the same item
        public virtual void AddStaticEffect(CharacterManager character)
        {
            // Add Effect Icon on HUD if character is player
            PlayerManager player = character as PlayerManager;
            if (player != null)
            {
                SetAllStaticEffectIcons(player);
            }
        }

        public virtual void RemoveStaticEffect(CharacterManager character)
        {
            // Removes Effect Icon on HUD if character is player
            PlayerManager player = character as PlayerManager;
            if (player != null)
            {
                RemoveAllStaticEffectIcons(player);
            }
        }

        public virtual void SetAllStaticEffectIcons(PlayerManager player)
        {
            if (!player.uIManager.staticEffects.activeInHierarchy)
            {
                player.uIManager.staticEffects.SetActive(true);
            }

            if (player.uIManager.staticEffects01UI.effectImage.sprite == null)
            {
                player.uIManager.staticEffects01UI.SetEffectIcon(effectIcon);
            }
            else if (player.uIManager.staticEffects02UI.effectImage.sprite == null)
            {
                player.uIManager.staticEffects02UI.gameObject.SetActive(true);
                player.uIManager.staticEffects02UI.SetEffectIcon(effectIcon);
            }
            else if (player.uIManager.staticEffects03UI.effectImage.sprite == null)
            {
                player.uIManager.staticEffects03UI.SetEffectIcon(effectIcon);
            }
            else if (player.uIManager.staticEffects04UI.effectImage.sprite == null)
            {
                player.uIManager.staticEffects04UI.SetEffectIcon(effectIcon);
            }
            else if (player.uIManager.staticEffects05UI.effectImage.sprite == null)
            {
                player.uIManager.staticEffects05UI.SetEffectIcon(effectIcon);
            }
            else if (player.uIManager.staticEffects06UI.effectImage.sprite == null)
            {
                player.uIManager.staticEffects06UI.SetEffectIcon(effectIcon);
            }
            else if (player.uIManager.staticEffects07UI.effectImage.sprite == null)
            {
                player.uIManager.staticEffects07UI.SetEffectIcon(effectIcon);
            }
        }

        public virtual void RemoveAllStaticEffectIcons(PlayerManager player)
        {
            if (player.characterEffectsManager.GetStaticEffectsCount() == 7)
            {
                player.uIManager.staticEffects07UI.SetEffectIcon(null);
                player.uIManager.staticEffects07UI.gameObject.SetActive(false);
            }
            else if (player.characterEffectsManager.GetStaticEffectsCount() == 6)
            {
                player.uIManager.staticEffects06UI.SetEffectIcon(null);
                player.uIManager.staticEffects06UI.gameObject.SetActive(false);
            }
            else if (player.characterEffectsManager.GetStaticEffectsCount() == 5)
            {
                player.uIManager.staticEffects05UI.SetEffectIcon(null);
                player.uIManager.staticEffects05UI.gameObject.SetActive(false);
            }
            else if (player.characterEffectsManager.GetStaticEffectsCount() == 4)
            {
                player.uIManager.staticEffects04UI.SetEffectIcon(null);
                player.uIManager.staticEffects04UI.gameObject.SetActive(false);
            }
            else if (player.characterEffectsManager.GetStaticEffectsCount() == 3)
            {
                player.uIManager.staticEffects03UI.SetEffectIcon(null);
                player.uIManager.staticEffects03UI.gameObject.SetActive(false);
            }
            else if (player.characterEffectsManager.GetStaticEffectsCount() == 2)
            {
                player.uIManager.staticEffects02UI.SetEffectIcon(null);
                player.uIManager.staticEffects02UI.gameObject.SetActive(false);
            }
            else if (player.characterEffectsManager.GetStaticEffectsCount() == 1)
            {
                player.uIManager.staticEffects01UI.SetEffectIcon(null);
                player.uIManager.staticEffects.SetActive(false);
            }
        }
    }
}
