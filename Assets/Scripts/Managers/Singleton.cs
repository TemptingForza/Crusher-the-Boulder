using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    public Singleton()
    {
        if (this is T value)
            Instance = value;
    }
    static T instance;
    public static T Instance {
        get => instance && instance.gameObject.activeInHierarchy ? instance : null;
        set => instance = value;
    }
}
