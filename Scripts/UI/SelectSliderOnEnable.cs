using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AG
{
    public class SelectSliderOnEnable : MonoBehaviour
    {
        public Slider statSlider;

        void OnEnable() 
        {
            statSlider.Select();
            statSlider.OnSelect(null);
        }
    }
}
