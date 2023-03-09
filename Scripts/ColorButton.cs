using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public SelectHairColor selectHairColor;
    public SelectEyebrowColor selectEyebrowColor;
    public SelectFacialHairColor selectFacialHairColor;
    public SelectSkinColor selectSkinColor;
    public SelecEyeColor selectEyeColor;
    public SelectMarksColor selectMarksColor;

    [Header("Color Values")]
    public float redAmount;
    public float greenAmount;
    public float blueAmount;

    [Header("Color Sliders")]
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;

    public Image colorImage;

    void Awake() 
    {
        colorImage = GetComponent<Image>();
        redAmount = colorImage.color.r;
        greenAmount = colorImage.color.g;
        blueAmount = colorImage.color.b;
    }

    public void SetSliderValuesToImageColor()
    {
        redSlider.value = redAmount;
        greenSlider.value = greenAmount;
        blueSlider.value = blueAmount;

        selectHairColor.redAmount = redAmount;
        selectHairColor.greenAmount = greenAmount;
        selectHairColor.blueAmount = blueAmount;
        selectHairColor.SetHairColor();
    }

    public void SetEyebrowsSliderValuesToImageColor()
    {
        redSlider.value = redAmount;
        greenSlider.value = greenAmount;
        blueSlider.value = blueAmount;

        selectEyebrowColor.redAmount = redAmount;
        selectEyebrowColor.greenAmount = greenAmount;
        selectEyebrowColor.blueAmount = blueAmount;
        selectEyebrowColor.SetEyebrowColor();
    }

    public void SetFacialHairSliderValuesToImageColor()
    {
        redSlider.value = redAmount;
        greenSlider.value = greenAmount;
        blueSlider.value = blueAmount;

        selectFacialHairColor.redAmount = redAmount;
        selectFacialHairColor.greenAmount = greenAmount;
        selectFacialHairColor.blueAmount = blueAmount;
        selectFacialHairColor.SetFacialHairColor();
    }

    public void SetEyesSliderValuesToImageColor()
    {
        redSlider.value = redAmount;
        greenSlider.value = greenAmount;
        blueSlider.value = blueAmount;

        selectEyeColor.redAmount = redAmount;
        selectEyeColor.greenAmount = greenAmount;
        selectEyeColor.blueAmount = blueAmount;
        selectEyeColor.SetEyeColor();
    }

    public void SetSkinSliderValuesToImageColor()
    {
        redSlider.value = redAmount;
        greenSlider.value = greenAmount;
        blueSlider.value = blueAmount;

        selectSkinColor.redAmount = redAmount;
        selectSkinColor.greenAmount = greenAmount;
        selectSkinColor.blueAmount = blueAmount;
        selectSkinColor.SetSkinColor();
    }

    public void SetMarksSliderValuesToImageColor()
    {
        redSlider.value = redAmount;
        greenSlider.value = greenAmount;
        blueSlider.value = blueAmount;

        selectMarksColor.redAmount = redAmount;
        selectMarksColor.greenAmount = greenAmount;
        selectMarksColor.blueAmount = blueAmount;
        selectMarksColor.SetMarkColor();
    }
}
