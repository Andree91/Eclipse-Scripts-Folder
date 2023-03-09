using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PoisonSurface : MonoBehaviour
    {
        //public float poisonBuildUpAmount = 7;

        public List<CharacterManager> charactersInsidePoisonSurface;

        void OnTriggerEnter(Collider other)
        {
            //if (other.gameObject.tag == "Animal" || other.gameObject.tag == "Interactable") { return; }
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
            {
                Debug.Log($"This Character is inside poison surface: {other.gameObject.name}");
                if (!character.isRiding && !character.isImmuneToPoison)
                {
                    charactersInsidePoisonSurface.Add(character);
                }
            }
        }

        void OnTriggerStay(Collider other)
        {
            foreach (CharacterManager character in charactersInsidePoisonSurface)
            {
                if (character.characterStatsManager.isPoisoned) { return; }//continue; }

                PoisonBuildUpEffect poisonBuildUp = Instantiate(WorldCharacterEffectsManager.instance.poisonBuildUpEffect);

                foreach (var effect in character.characterEffectsManager.timedEffects)
                {
                    if (effect.effectID == poisonBuildUp.effectID) { return; }
                }

                character.characterEffectsManager.timedEffects.Add(poisonBuildUp);

                //character.poisonBuildUp = character.poisonBuildUp + poisonBuildUpAmount * Time.deltaTime;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
            {
                charactersInsidePoisonSurface.Remove(character);
            }
        }
    }
}
