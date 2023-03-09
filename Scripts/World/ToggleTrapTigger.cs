using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class ToggleTrapTigger : MonoBehaviour
    {
        public BoxCollider triggerCollider;
        TrapWallArrow trigger;

        void Awake() 
        {
            //triggerCollider = GetComponentInChildren<BoxCollider>();
            trigger = GetComponentInChildren<TrapWallArrow>();
        }

        public void DisableTrapTrigger()
        {
            //triggerCollider.enabled = false;
            trigger.isEnabled = false;
        }

        public void EnableTrapTrigger()
        {
            //triggerCollider.enabled = true;
            trigger.isEnabled = true;
        }
    }
}
