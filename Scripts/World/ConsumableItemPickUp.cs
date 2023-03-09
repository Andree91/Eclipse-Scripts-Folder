using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class ConsumableItemPickUp : Interactable
    {
        [Header("Item information")]
        [SerializeField] int itemPickUpID; // This is unique  ID for this item spawn in the game world, each item you place in your world should have it's own UNIQUE ID
        [SerializeField] bool hasBeenLooted;

        [Header("Item")]
        public ConsumableItem item;
        public int amount = 1;
        List<ConsumableItem> quickSlotItems;
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
            player.uIManager.quickSlotsUI.UpdateCurrentConsumableIcon(player.playerInventoryManager.currentConsumable);
            player.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
            player.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = item.itemIcon.texture;
            if (amount > 1)
            {
                player.itemInteractableGameObject.GetComponentInChildren<RawImage>().gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "x " + amount.ToString();
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
            if (!playerInventory.consumablesInventory.Contains(item))
            {
                quickSlotItems = new List<ConsumableItem>();
                for (int i = 0; i < playerInventory.consumablesInQuickSlots.Length; i++)
                {
                    quickSlotItems.Add(playerInventory.consumablesInQuickSlots[i]);
                }

                if (!quickSlotItems.Contains(item))
                {
                    playerInventory.consumablesInventory.Add(item);
                    isInInventory = true;
                    Debug.Log("Now item is in Inventory items");
                }
            }
            else
            {
                isInInventory = true;
            }

            if (isInInventory)
            {
                for (int i = 0; i < playerInventory.consumablesInventory.Count; i++)
                {
                    if (playerInventory.consumablesInventory[i] != null)
                    {
                        if (playerInventory.consumablesInventory[i].itemName == item.itemName)
                        {
                            playerInventory.consumablesInventory[i].currentItemAmount += amount;

                            if (playerInventory.consumablesInventory[i].currentItemAmount > item.maxItemAmount)
                            {
                                playerInventory.consumablesInventory[i].currentStoredItemAmount += playerInventory.consumablesInventory[i].currentItemAmount - item.maxItemAmount;

                                if (playerInventory.consumablesInventory[i].currentStoredItemAmount > item.maxStoredAmount)
                                {
                                    playerInventory.consumablesInventory[i].currentStoredItemAmount = item.maxStoredAmount;
                                }

                                playerInventory.consumablesInventory[i].currentItemAmount = item.maxItemAmount;
                            }
                            break;
                        }
                    }
                }
            }


            if (!isInInventory)
            {
                for (int i = 0; i < quickSlotItems.Count; i++)
                {
                    if (quickSlotItems[i] != null)
                    {
                        if (quickSlotItems[i].itemName == item.itemName)
                        {
                            quickSlotItems[i].currentItemAmount += amount;

                            if (quickSlotItems[i].currentItemAmount > item.maxItemAmount)
                            {
                                quickSlotItems[i].currentStoredItemAmount += quickSlotItems[i].currentItemAmount - item.maxItemAmount;

                                if (quickSlotItems[i].currentStoredItemAmount > item.maxStoredAmount)
                                {
                                    quickSlotItems[i].currentStoredItemAmount = item.maxStoredAmount;
                                }

                                quickSlotItems[i].currentItemAmount = item.maxItemAmount;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
