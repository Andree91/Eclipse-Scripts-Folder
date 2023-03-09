using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class StaticEffectsUI : MonoBehaviour
    {
        public Image effectImage;

        public void SetEffectIcon(Sprite icon)
        {
            if (icon != null)
            {
                effectImage.sprite = icon;
            }
            else
            {
                effectImage.sprite = null;
            }
        }
    }
}
