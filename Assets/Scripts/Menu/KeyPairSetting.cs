using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPairSetting : Setting<(bool isMain, KeyCode newValue)>
{
    public KeyInput mainInput;
    public KeyInput altInput;
    void Awake()
    {
        mainInput.onValueChange += x => RaiseEvent((true, x));
        altInput.onValueChange += x => RaiseEvent((false, x));
    }
}
