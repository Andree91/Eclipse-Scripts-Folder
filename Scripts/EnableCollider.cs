using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class EnableCollider : MonoBehaviour
    {
        public float timeUntilEnable = 2f;
        Collider _collider;
        CharacterManager character;

        List<BoxCollider> weapons;

        void Awake() 
        {
            _collider = GetComponent<Collider>();
            character = GetComponentInParent<CharacterManager>();
        }

        void Update() 
        {
            if (_collider.enabled == false)
            {
                StartCoroutine(EnableThisCollider(timeUntilEnable));
            }
        }

        IEnumerator EnableThisCollider(float timeUntilEnable)
        {
            yield return new WaitForSeconds(timeUntilEnable);
            Debug.Log("Now collider should be true");
            _collider.enabled = true;
            character.isUsingShieldSpell = true;

        }

        void OnDestroy() 
        {
            character.isUsingShieldSpell = false;
        }

        // void OnCollisionEnter(Collision other) 
        // {
        //     if (other.gameObject.layer == LayerMask.NameToLayer("Damage Collider"))
        //     {
        //         BoxCollider weapon = other.gameObject.GetComponent<BoxCollider>();

        //         weapons.Add(weapon);
        //     }

        //     // if (other.gameObject.tag == "Weapon")
        //     // {
        //     //     Debug.Log("Entering from damage collider");
        //     //     other.gameObject.GetComponent<Collider>().enabled = false;
        //     // }
        // }

        // void OnCollisionStay(Collision other) 
        // {
        //     if (other.gameObject.layer == 13)
        //     {

        //         BoxCollider weapon = other.gameObject.GetComponent<BoxCollider>();

        //         weapons.Add(weapon);

        //         foreach (BoxCollider weaponCollider in weapons)
        //         {
        //             Debug.Log("Staying from damage collider");
        //             weapon.enabled = false;
        //         }
        //     }

        //     // if (other.gameObject.tag == "Weapon")
        //     // {
        //     //     Debug.Log("Entering from damage collider");
        //     //     other.gameObject.GetComponent<Collider>().enabled = false;
        //     // }
        // }
    }
}
