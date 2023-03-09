using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [CreateAssetMenu(menuName = "Items/Equipment/Hand Equipment")]
    public class HandEquipment : EquipmentItem
    {
        public string leftLowerArmModelName;
        public string leftHandModelName;
        public string rightLowerArmModelName;
        public string rightArmModelName;
    }
}
