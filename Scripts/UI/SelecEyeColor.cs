using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelecEyeColor : MonoBehaviour
{
    [Header("Color Values")]
    public float redAmount;
    public float greenAmount;
    public float blueAmount;
    //public float alphaAmount;

    [Header("Color Sliders")]
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    //public Slider alphaSlider;

    Color currentEyeColor;

    //We grab the material from skin mess renderer and change the color properties of the material
    public List<SkinnedMeshRenderer> rendererList = new List<SkinnedMeshRenderer>();

    public void UpdateSliders()
    {
        redAmount = redSlider.value;
        greenAmount = greenSlider.value;
        blueAmount = blueSlider.value;
        SetEyeColor();
    }

    public void SetEyeColor()
    {
        currentEyeColor = new Color(redAmount, greenAmount, blueAmount);//, alphaAmount);

        for (int i = 0; i < rendererList.Count; i++)
        {
            //If usin SYNTY models
            rendererList[i].material.SetColor("_Color_Eyes", currentEyeColor);

            //If using regular models
            //rendererList[i].material.SetColor("_Color", currentHairColor);
        }

        //eyeRenderer.material.SetColor("_Color_Hair", currentEyeColor);
    }
}
