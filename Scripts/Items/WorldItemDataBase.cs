using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AG
{ 
    public class WorldItemDataBase : MonoBehaviour
    {
        public static WorldItemDataBase Instance;

        public List<WeaponItem> weaponItems = new List<WeaponItem>();

        public List<EquipmentItem> equipmentItems = new List<EquipmentItem>();

        public List<AmuletItem> amuletItems = new List<AmuletItem>();

        public List<ConsumableItem> consumableItems = new List<ConsumableItem>();

        public List<RangedAmmoItem> rangedAmmoItems = new List<RangedAmmoItem>();

        void Awake() 
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public WeaponItem GetWeaponItemByID(int weaponID)
        {
            // Search for the first weapon on weapoinItems list that matches with weaponID
            return weaponItems.FirstOrDefault(weapon => weapon.itemID == weaponID);
        }

        public EquipmentItem GetEquipmentItemByID(int equipmentID)
        {
            // Search for the first equipment on equipmentItems list that matches with equipmentID
            return equipmentItems.FirstOrDefault(equipment => equipment.itemID == equipmentID);
        }

        public AmuletItem GetAmuletItemByID(int amuletID)
        {
            // Search for the first amulet on amuletItems list that matches with amuletID
            return amuletItems.FirstOrDefault(amulet => amulet.itemID == amuletID);
        }

        public ConsumableItem GetConsumableItemByID(int consumableID)
        {
            // Search for the first consumable on consumableItems list that matches with consumableID
            return consumableItems.FirstOrDefault(consumable => consumable.itemID == consumableID);
        }

        public RangedAmmoItem GetRangedAmmoItemByID(int rangedAmmoID)
        {
            // Search for the first ranged ammo on rangedAmmoItems list that matches with rangedAmmoID
            return rangedAmmoItems.FirstOrDefault(rangedAmmo => rangedAmmo.itemID == rangedAmmoID);
        }
    }
}
