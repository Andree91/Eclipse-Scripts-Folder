using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class DamagePlayer : MonoBehaviour
    {
        public int damage = 0;
        public bool isSwingBlade;
        public GameObject animalDamageTrigger;

        public List<CharacterManager> charactersDamagedDuringThisCalculation = new List<CharacterManager>();

        void OnCollisionEnter(Collision other) 
        {
            if (other.gameObject.tag == "Animal")
            {
                animalDamageTrigger.SetActive(true);
            }

            CharacterManager character = other.gameObject.GetComponentInParent<CharacterManager>();

            if (character != null)
            {
                if (charactersDamagedDuringThisCalculation.Contains(character)) { return; }

                charactersDamagedDuringThisCalculation.Add(character);

                Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                character.characterEffectsManager.PlayBloodSplatterFX(contactPoint);
                character.characterStatsManager.TakeDamage(damage, 0, 0, "Damage_Right_01", null);
                Rigidbody targetRigidbody = other.gameObject.GetComponent<Rigidbody>();
                if (isSwingBlade)
                {
                    targetRigidbody.AddExplosionForce(500, contactPoint, 1, 0.7f, ForceMode.Impulse);
                }
                StartCoroutine(ClearcharactersDamagedDuringThisCalculation());
            }
        }

        void OnTriggerEnter(Collider other) 
        {   
            if (other.tag =="Animal")
            {
                animalDamageTrigger.SetActive(true);
            }

            if (other.gameObject.tag == "Character")
            {
                SphereCollider dialogueCollider = other.gameObject.GetComponent<SphereCollider>();
                if (dialogueCollider != null)
                {
                    if (dialogueCollider.enabled == true)
                    {
                        dialogueCollider.enabled = false;
                    }
                }
                if (isSwingBlade) { return; }
                CharacterManager character = other.GetComponentInParent<CharacterManager>();
                // if (charactersDamagedDuringThisCalculation.Contains(character)) { return; }

                // charactersDamagedDuringThisCalculation.Add(character);

                if (character != null)
                {
                    Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    character.characterEffectsManager.PlayBloodSplatterFX(contactPoint);
                    character.characterStatsManager.TakeDamage(damage, 0, 0, "Damage_Right_01", null);
                }
            }
        }

        void OnTriggerExit(Collider other) 
        {
            if (other.gameObject.tag == "Character")
            {
                SphereCollider dialogueCollider = other.gameObject.GetComponent<SphereCollider>();
                if (dialogueCollider != null)
                {
                    if (dialogueCollider.enabled == false)
                    {
                        dialogueCollider.enabled = true;
                    }
                }
            }
        }

        IEnumerator ClearcharactersDamagedDuringThisCalculation()
        {
            yield return new WaitForSeconds(1f);
            if (charactersDamagedDuringThisCalculation.Count > 0)
            {
                charactersDamagedDuringThisCalculation.Clear();
            }
        }
    }
}
