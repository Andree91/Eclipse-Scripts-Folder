using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class TorsoModelChanger : MonoBehaviour
    {
        public List<GameObject> torsoModels;

        void Awake() 
        {
            GetAllTorsoModels();
        }

        void GetAllTorsoModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                torsoModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnEquipAllTorsoModels()
        {
            foreach (GameObject torsoModel in torsoModels)
            {
                torsoModel.SetActive(false);
            }
        }

        public void EquipTorsoModelByName(string torsoName)
        {
            for (int i = 0; i < torsoModels.Count; i++)
            {
                if (torsoModels[i].name == torsoName)
                {
                    torsoModels[i].SetActive(true);
                }
            }
        }
    }
}
