using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoolSetting : Setting<bool>
{
    public Toggle checkbox;
    void Start()
    {
        checkbox.onValueChanged.AddListener(x => RaiseEvent(x));
    }
}
