using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class Setting<T> : MonoBehaviour
{
    public Text text;
    public event OnChange onValueChange;
    public delegate void OnChange(T newValue);
    protected void RaiseEvent(T param) => onValueChange?.Invoke(param);
}
