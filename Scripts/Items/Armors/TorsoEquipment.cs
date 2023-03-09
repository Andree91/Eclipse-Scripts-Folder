using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Items/Equipment/Torso Equipment")]
    public class TorsoEquipment : EquipmentItem
    {
        public string torsoModelName;
        public string leftUpperArmModelName;
        public string rightUpperArmModelName;
        public string leftElbowPadModelName;
        public string rightElbowPadModelName;
        public string leftShoulderPadModelName;
        public string rightShoulderPadModelName;
        public string capeModelName;
    }
}
