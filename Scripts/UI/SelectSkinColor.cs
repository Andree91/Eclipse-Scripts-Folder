using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkinColor : MonoBehaviour
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

    Color currentSkinColor;

    public GameObject parentGameObject;
    public SkinnedMeshRenderer headRenderer;

    //We grab the material from skin mess renderer and change the color properties of the material
    public List<SkinnedMeshRenderer> rendererList = new List<SkinnedMeshRenderer>();

    public void UpdateSliders()
    {
        redAmount = redSlider.value;
        greenAmount = greenSlider.value;
        blueAmount = blueSlider.value;
        SetSkinColor();
    }

    public void SetSkinColor()
    {
        currentSkinColor = new Color(redAmount, greenAmount, blueAmount);//, alphaAmount);

        LocateCurrentHeadModel();
        for (int i = 0; i < rendererList.Count; i++)
        {
            if (i == 0)
            {
                rendererList[i] = headRenderer;
            }
            //If using SYNTY models
            rendererList[i].material.SetColor("_Color_Skin", currentSkinColor);

            //If using regular models
            //rendererList[i].material.SetColor("_Color", currentHairColor);
        }
    }

    void LocateCurrentHeadModel()
    {
        for (int i = 0; i < parentGameObject.transform.childCount; i++)
        {
            var child = parentGameObject.transform.GetChild(i).gameObject;

            if (child != null)
            {
                if (child.activeInHierarchy)
                {
                    headRenderer = child.GetComponent<SkinnedMeshRenderer>();
                    break;
                }
            }
        }
    }
}
