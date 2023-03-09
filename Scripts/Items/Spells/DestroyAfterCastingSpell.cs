using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class DestroyAfterCastingSpell : MonoBehaviour
    {
        CharacterManager characterCastingSpell;

        void Awake() 
        {
            characterCastingSpell = GetComponentInParent<CharacterManager>();
        }

        void Update() 
        {
            if (characterCastingSpell.isFiringSpell)
            {
                Destroy(gameObject);
            }
        }
    }
}
