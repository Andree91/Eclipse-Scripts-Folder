using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalEyesChanger : MonoBehaviour
{
    public Animator animalEyesAnimator;
    public int eyeParametrValue;

    public void SetAnimalEyesParametrs(int eyeParametr)
    {
        animalEyesAnimator.SetInteger("Eyes", eyeParametr);
    }
}
