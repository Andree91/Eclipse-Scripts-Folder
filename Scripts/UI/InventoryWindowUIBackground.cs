using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class InventoryWindowUIBackground : MonoBehaviour
    {
        public UIManager uIManager;
        [SerializeField] RectTransform rt;
        [SerializeField] Vector3 originalRtPos;
        [SerializeField] Vector2 originalRtDelta;

        // This is not relevant right now, so I'm not using it

        public void ChangeSizeOfUIBackgroundToOriginal() 
        {
            if (rt != null)
            {
                rt.sizeDelta = new Vector2(originalRtDelta.x, originalRtDelta.y);
                rt.position = new Vector3(originalRtPos.x, originalRtPos.y, originalRtPos.z);
            }
        }

        public void ChangeSizeOfUIBackground()
        {
            originalRtPos = new Vector3(rt.position.x, rt.position.y, rt.position.z);
            originalRtDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y);
            rt.sizeDelta = new Vector2(originalRtDelta.x, 1004.938f);
            rt.position = new Vector3(originalRtPos.x, originalRtPos.y + 41, originalRtPos.z);
        }
    }
}
