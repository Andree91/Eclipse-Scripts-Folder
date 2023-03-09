using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WaterInteractable : Interactable
    {
        public Vector3 swimmingLevel;

        public override void Interact(PlayerManager player)
        {
            //Destroy(player.playerEffectsManager.currentWaterParticleFX);
            player.DiveUnderWater(swimmingLevel);
            player.playerEffectsManager.PlayUnderWaterBubbleFX(player.lockOnTransform);
        }
    }
}
