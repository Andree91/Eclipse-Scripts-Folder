using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class RangedAmmoItemPickUp : Interactable
    {
        [Header("Item information")]
        [SerializeField] int itemPickUpID; // This is unique  ID for this item spawn in the game world, each item you place in your world should have it's own UNIQUE ID
        [SerializeField] bool hasBeenLooted;

        [Header("Item")]
        public RangedAmmoItem item;
        public int amount = 1;
        List<RangedAmmoItem> ammoSlotItems;
        bool isInInventory;

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
            AddItemToInventory(playerInventory);

            if (item.itemName == player.playerInventoryManager.currentAmmo01.itemName)
            {
                player.uIManager.quickSlotsUI.UpdateAmmoQuickSlotsUI(player.playerInventoryManager.currentAmmo01, true);
            }

            if (item.itemName == player.playerInventoryManager.currentAmmo02.itemName)
            {
                player.uIManager.quickSlotsUI.UpdateAmmoQuickSlotsUI(player.playerInventoryManager.currentAmmo01, false);
            }

            player.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
            player.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = item.itemIcon.texture;
            if (amount > 1)
            {
                player.itemInteractableGameObject.GetComponentInChildren<RawImage>().gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "x" + amount.ToString();
            }
            else
            {
                player.itemInteractableGameObject.GetComponentInChildren<RawImage>().gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            player.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }

        void AddItemToInventory(PlayerInventoryManager playerInventory)
        {
            if (!playerInventory.rangedAmmoItemsInventory.Contains(item))
            {
                ammoSlotItems = new List<RangedAmmoItem>();
                for (int i = 0; i < playerInventory.rangedAmmoItemsInAmmoSlots.Length; i++)
                {
                    ammoSlotItems.Add(playerInventory.rangedAmmoItemsInAmmoSlots[i]);
                }

                if (!ammoSlotItems.Contains(item))
                {
                    playerInventory.rangedAmmoItemsInventory.Add(item);
                    isInInventory = true;
                }
            }
            else
            {
                isInInventory = false;
            }

            if (isInInventory)
            {
                // Have to check later if there are no item in inventory or ammoslot, but you still managed to pick up like 1000 pieces of that ammo
                for (int i = 0; i < playerInventory.rangedAmmoItemsInventory.Count; i++)
                {
                    if (playerInventory.rangedAmmoItemsInventory[i].itemName == item.itemName)
                    {
                        playerInventory.rangedAmmoItemsInventory[i].currentAmmo += amount;

                        if (playerInventory.rangedAmmoItemsInventory[i].currentAmmo > item.carryLimit)
                        {
                            playerInventory.rangedAmmoItemsInventory[i].currentStoredAmmoAmount += playerInventory.rangedAmmoItemsInventory[i].currentAmmo - item.carryLimit;

                            if (playerInventory.rangedAmmoItemsInventory[i].currentStoredAmmoAmount > item.maxStoredAmmoAmount)
                            {
                                playerInventory.rangedAmmoItemsInventory[i].currentStoredAmmoAmount = item.maxStoredAmmoAmount;
                            }

                            playerInventory.rangedAmmoItemsInventory[i].currentAmmo = item.carryLimit;
                        }
                        break;
                    }
                }
            }

            if (!isInInventory)
            {
                for (int i = 0; i < ammoSlotItems.Count; i++)
                {
                    if (ammoSlotItems[i].itemName == item.itemName)
                    {
                        ammoSlotItems[i].currentAmmo += amount;

                        if (ammoSlotItems[i].currentAmmo > item.carryLimit)
                        {
                            ammoSlotItems[i].currentStoredAmmoAmount += ammoSlotItems[i].currentAmmo - item.carryLimit;

                            if (ammoSlotItems[i].currentStoredAmmoAmount > item.maxStoredAmmoAmount)
                            {
                                ammoSlotItems[i].currentStoredAmmoAmount = item.maxStoredAmmoAmount;
                            }

                            ammoSlotItems[i].currentAmmo = item.carryLimit;
                        }
                        break;
                    }
                }
            }
        }
    }
}
