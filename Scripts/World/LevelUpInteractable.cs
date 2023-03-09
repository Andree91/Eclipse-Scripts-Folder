using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class LevelUpInteractable : Interactable
    {
        public override void Interact(PlayerManager player)
        {
            base.Interact(player);
            //player.playerEffectsManager.ResetAllBuildUpEffects();
            player.uIManager.levelUpWindow.SetActive(true);
            player.SitAtCampFireInteraction(transform);
        }
    }
}
