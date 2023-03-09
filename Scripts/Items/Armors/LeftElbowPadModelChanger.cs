using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class LeftElbowPadModelChanger : MonoBehaviour
    {
        public List<GameObject> elbowPadModels;

        void Awake()
        {
            GetAllElbowPadModels();
        }

        void GetAllElbowPadModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                elbowPadModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllElbowPadModels()
        {
            foreach (GameObject torsoModel in elbowPadModels)
            {
                torsoModel.SetActive(false);
            }
        }

        public void EquipElbowPadModelByName(string elbowPadName)
        {
            for (int i = 0; i < elbowPadModels.Count; i++)
            {
                if (elbowPadModels[i].name == elbowPadName)
                {
                    elbowPadModels[i].SetActive(true);
                }
            }
        }
    }
}
