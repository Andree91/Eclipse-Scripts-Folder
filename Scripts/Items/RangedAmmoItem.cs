using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Items/Ammo")]
    public class RangedAmmoItem : Item
    {
        [Header("Ammo type")]
        public AmmoType ammotype;
        public bool isEmpty;

        [Header("Ammo Velocity")]
        public float forwardVelocity = 550;
        public float upwardVelocity = 0;
        public float ammoMass = 0;
        public bool useGravity = false;

        [Header("Ammo Capacity")]
        public int carryLimit = 99;
        public int currentAmmo = 99;
        public int maxStoredAmmoAmount;
        public int currentStoredAmmoAmount;

        [Header("Ammo Base Damage ")]
        public int physicalDamage = 50;
        public int magicDamage;
        public int fireDamage;
        public int lightningDamage;
        public int holyDamage;

        [Header("Item Models")]
        public GameObject loadedItemModel; // the model that instantiate at player hand while doing draw ht bow animation
        public GameObject liveAmmomodel; //Model which has rb and collider, flyis with velocity stats
        public GameObject penetratedModel; //Instantiate at target body/surface at specific parts while colliding


    }
}
