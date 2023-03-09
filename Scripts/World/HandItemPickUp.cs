using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class HandItemPickUp : Interactable
    {
        [Header("Item information")]
        [SerializeField] int itemPickUpID; // This is unique  ID for this item spawn in the game world, each item you place in your world should have it's own UNIQUE ID
        [SerializeField] bool hasBeenLooted;

        [Header("Item")]
        public HandEquipment item;

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
            player.playerMovement.rb.velocity = Vector3.zero; //Stop player movement during pickup
            player.playerAnimatorManager.PlayTargetAnimation("Pick Up Item", true);
            player.playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
            player.playerWeaponSlotManager.leftHandSlot.UnloadWeapon();
            player.playerEffectsManager.LoadBothWeaponsOnTimer();
            player.playerInventoryManager.handEquipmentInventory.Add(item);
            player.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
            player.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = item.itemIcon.texture;
            player.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
