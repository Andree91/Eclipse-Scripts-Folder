using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] float timeUntilDestroyed = 2;

        void Awake() 
        {
            Destroy(gameObject, timeUntilDestroyed);
        }
    }
}
