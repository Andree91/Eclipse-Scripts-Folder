using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class BloodStainInteractable : Interactable
    {
        [Header("Item information")]
        [SerializeField] int itemPickUpID; // This is unique  ID for this item spawn in the game world, each item you place in your world should have it's own UNIQUE ID
        public bool hasBeenLooted;

        public Sprite soulImage;

        protected override void Start()
        {
            base.Start();

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
                Debug.Log("Now bloodstain is removed");
            }

            // Saves the pick up to our save data so it doesn't spawn again when we re-load the area
            WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Add(itemPickUpID, true);

            hasBeenLooted = true;

            // Place the item in player's inventory
            PicUpItem(player);
        }

        void PicUpItem(PlayerManager player)
        {
            player.uIManager.hud.soulCount.SetActive(true);
            player.playerMovement.rb.velocity = Vector3.zero; //Stop player movement during pickup
            player.playerAnimatorManager.PlayTargetAnimation("Pick Up Item", true);
            player.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetInt("lostSoulsCount").ToString();
            player.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = soulImage.texture;
            player.itemInteractableGameObject.SetActive(true);
            player.playerStatsManager.currentSoulCount += PlayerPrefs.GetInt("lostSoulsCount");
            SoulCountBar soulCountBar = player.uIManager.hud.soulCount.GetComponent<SoulCountBar>();
            soulCountBar.SetSoulCountText(player.playerStatsManager.currentSoulCount);
            player.HideSoulCountHUD();
            gameObject.SetActive(false);
        }
    }
}
