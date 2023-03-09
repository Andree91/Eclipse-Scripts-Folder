using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class WeaponPickUp : Interactable
    {
        [Header("Item information")]
        [SerializeField] int itemPickUpID; // This is unique  ID for this item spawn in the game world, each item you place in your world should have it's own UNIQUE ID
        [SerializeField] bool hasBeenLooted;

        [Header("Item")]
        public WeaponItem weapon;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            if (isLootItem)
            {
                itemPickUpID = WorldSaveGameManager.instance.currentCharacterSaveData.lastInstantiateLootItemID + 1;
                WorldSaveGameManager.instance.currentCharacterSaveData.lastInstantiateLootItemID = itemPickUpID;
            }

            // If the saves data doesn't contais this item, we haven't looted it yet, so we add it to the list it as NOT LOOTED
            if (!WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.ContainsKey(itemPickUpID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Add(itemPickUpID, false);
            }

            hasBeenLooted = WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld[itemPickUpID];

            if (hasBeenLooted)
            {
                gameObject.SetActive(false);
            }
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            // Notify the character data this item has been looted from the world, so it doesn't spawn again
            if (WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.ContainsKey(itemPickUpID))
            {
                WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Remove(itemPickUpID);
            }

            // Saves the pick up to our save data so it doesn't spawn again when we re-load the area
            WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Add(itemPickUpID, true);

            hasBeenLooted = true;

            // Place the item in player's inventory
            PicUpItem(player);
        }

        void PicUpItem(PlayerManager player)
        {
            PlayerInventoryManager playerInventory;
            PlayerMovement playerMovement;
            PlayerAnimatorManager animatorHandler;

            playerInventory = player.GetComponent<PlayerInventoryManager>();
            playerMovement = player.GetComponent<PlayerMovement>();
            animatorHandler = player.GetComponentInChildren<PlayerAnimatorManager>();

            playerMovement.rb.velocity = Vector3.zero; //Stop player movement during pickup
            animatorHandler.PlayTargetAnimation("Pick Up Item", true);
            player.playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
            player.playerWeaponSlotManager.leftHandSlot.UnloadWeapon();
            player.playerEffectsManager.LoadBothWeaponsOnTimer();
            playerInventory.weaponsInventory.Add(weapon);
            player.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName;
            player.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            player.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
