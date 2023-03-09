using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class FogWall : MonoBehaviour
    {
        void Awake() 
        {
            //DeactivateFogWall();
            ActivateFogWall();
        }

        public void ActivateFogWall()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateFogWall()
        {
            gameObject.SetActive(false);
        }
    }
}
