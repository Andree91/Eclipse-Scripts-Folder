using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  AG
{
    public class Destructible : MonoBehaviour
    {
        [Header("Breable Prop information")]
        [SerializeField] int propWorldID; // This is unique  ID for this prop in the game world, each breable prop you place in your world should have it's own UNIQUE ID
        [SerializeField] bool hasBeenBroken;

        public GameObject destroyedVersion;
        public GameObject pickUpItem;
        public ConsumableItem item;
        [SerializeField] int itemAmount;
        public Transform parentTransform;
        public bool hasItem;
        public bool useLocalScale = true;
        bool hasCollied;
        MeshCollider propsCollider;

        void Awake() 
        {
            if (parentTransform == null)
            {
                parentTransform = FindObjectOfType<BrokenProps>().transform;
            }

            propsCollider = GetComponent<MeshCollider>();
        }

        void Start() 
        {
            // If the saves data doesn't contais this prop, we haven't break it yet, so we add it to the list it as NOT DESTROID
            if (!WorldSaveGameManager.instance.currentCharacterSaveData.propsDestroid.ContainsKey(propWorldID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.propsDestroid.Add(propWorldID, false);
            }

            hasBeenBroken = WorldSaveGameManager.instance.currentCharacterSaveData.propsDestroid[propWorldID];

            if (hasBeenBroken)
            {
                gameObject.SetActive(false);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (hasCollied) { return; }
            if (!other.isTrigger) {return; }
            if (other.gameObject.tag == "Weapon")
            {
                Debug.Log("name of olider " + other.gameObject.name);
                hasCollied = true;
                NotifyToCharacterData();
                GameObject liveDestroydVersion = Instantiate(destroyedVersion, transform.position, transform.rotation, parentTransform);
                if (useLocalScale)
                {
                    liveDestroydVersion.transform.localScale = gameObject.transform.localScale;
                    //liveDestroydVersion.transform.localScale = gameObject.GetComponent<Renderer>().bounds.size; // This was working better for using real size of the gameObject
                }
                if (hasItem)
                {
                    GameObject itemLive = Instantiate(pickUpItem, transform.position, transform.rotation);
                    ConsumableItemPickUp itemPickup = itemLive.GetComponent<ConsumableItemPickUp>();
                    if (itemPickup != null)
                    {
                        if (item != null)
                        {
                            itemPickup.item = item;
                            itemPickup.amount = itemAmount;
                        }
                        itemPickup.isLootItem = true;
                    }
                }
                Destroy(gameObject);
            }
        }

        void OnCollisionEnter(Collision collision) 
        {
            if (hasCollied) { return; }
            if (collision.gameObject.tag == "Weapon")
            {
                Debug.Log("name of olider " + collision.gameObject.name);
                hasCollied = true;
                NotifyToCharacterData();
                GameObject liveDestroydVersion = Instantiate(destroyedVersion, transform.position, transform.rotation, parentTransform);
                if (useLocalScale)
                {
                    liveDestroydVersion.transform.localScale = gameObject.transform.localScale;
                    //liveDestroydVersion.transform.localScale = gameObject.GetComponent<Renderer>().bounds.size; // This was working better for using real size of the gameObject
                }
                if (hasItem)
                {
                    Instantiate(pickUpItem, transform.position, transform.rotation);
                    GameObject itemLive = Instantiate(pickUpItem, transform.position, transform.rotation);
                    ConsumableItemPickUp itemPickup = itemLive.GetComponent<ConsumableItemPickUp>();
                    if (itemPickup != null)
                    {
                        itemPickup.isLootItem = true;
                    }
                }
                Destroy(gameObject);
            }
        }

        void NotifyToCharacterData()
        {
            // Notify the character data that this prop has been destroid from the world, so it doesn't load it again.
            if (WorldSaveGameManager.instance.currentCharacterSaveData.propsDestroid.ContainsKey(propWorldID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.propsDestroid.Remove(propWorldID);
            }

            // Saves the prop state to our save data so it doesn't load again when we re-load the area
            WorldSaveGameManager.instance.currentCharacterSaveData.propsDestroid.Add(propWorldID, true);

            hasBeenBroken = true;
        }
    }
}
