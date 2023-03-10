using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIInputReciever))]
public class UIButton : Button
{
    InputReciever reciever;

    protected override void Awake() 
    {
        base.Awake();
        reciever = GetComponent<UIInputReciever>();
        onClick.AddListener(() => reciever.OnInputRecieved());
    }
}
