using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    public string Name;
    public Sprite Image;

    public abstract void OnActivate();
}
