using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class NameCharacter : MonoBehaviour
    {
        public CharacterStatsManager character;
        public TMP_InputField inputField;
        public TextMeshProUGUI nameButtonText;
        public TextMeshProUGUI playerNameSummaryText;

        public void NameMyCharacter()
        {
            character.characterName = inputField.text;

            if (character.characterName == "")
            {
                character.characterName = "Nameless";
            }

            nameButtonText.text = character.characterName;
            playerNameSummaryText.text = character.characterName;
        }
    }
}
