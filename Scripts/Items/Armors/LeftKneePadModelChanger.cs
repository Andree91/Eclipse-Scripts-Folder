using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class LeftKneePadModelChanger : MonoBehaviour
    {
        public List<GameObject> kneePadModels;

        void Awake()
        {
            GetAllKneePadModels();
        }

        void GetAllKneePadModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                kneePadModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllKneePadModels()
        {
            foreach (GameObject torsoModel in kneePadModels)
            {
                torsoModel.SetActive(false);
            }
        }

        public void EquipKneePadModelByName(string kneePadName)
        {
            for (int i = 0; i < kneePadModels.Count; i++)
            {
                if (kneePadModels[i].name == kneePadName)
                {
                    kneePadModels[i].SetActive(true);
                }
            }
        }
    }
}
