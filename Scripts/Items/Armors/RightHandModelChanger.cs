using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class RightHandModelChanger : MonoBehaviour
    {
        public List<GameObject> armModels;

        void Awake()
        {
            GetAllArmModels();
        }

        void GetAllArmModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                armModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllArmModelsModels()
        {
            foreach (GameObject armModel in armModels)
            {
                armModel.SetActive(false);
            }
        }

        public void EquipArmModelByName(string armName)
        {
            for (int i = 0; i < armModels.Count; i++)
            {
                if (armModels[i].name == armName)
                {
                    armModels[i].SetActive(true);
                }
            }
        }
    }
}
