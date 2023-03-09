using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class PointOfInterest : MonoBehaviour
    {
        public bool isPlayer;

        public void SetIsPlayerBool()
        {
            isPlayer = !isPlayer;
        }
    }
}
