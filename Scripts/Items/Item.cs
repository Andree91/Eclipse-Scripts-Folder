using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public int itemID;
        public Sprite itemIcon;
        public string itemName;
        public bool isKeyItem;

        [Header("Item Description")]
        [TextArea]
        public string itemDescription;
    }
}
