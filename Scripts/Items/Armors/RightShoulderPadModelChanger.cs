using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class RightShoulderPadModelChanger : MonoBehaviour
    {
        public List<GameObject> shoulderPadModels;

        void Awake()
        {
            GetAllShoulderPadModels();
        }

        void GetAllShoulderPadModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                shoulderPadModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllShoulderPadModels()
        {
            foreach (GameObject torsoModel in shoulderPadModels)
            {
                torsoModel.SetActive(false);
            }
        }

        public void EquipShoulderPadModelByName(string shoulderPadName)
        {
            for (int i = 0; i < shoulderPadModels.Count; i++)
            {
                if (shoulderPadModels[i].name == shoulderPadName)
                {
                    shoulderPadModels[i].SetActive(true);
                }
            }
        }
    }
}
