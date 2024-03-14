using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyPair", menuName = "ScriptableObjects/KeyPair", order = 1)]
public class KeyPair : ScriptableObject
{
    KeyPair() { }
    public static KeyPair Create(KeyCode Main = default, KeyCode Alt = default)
    {
        var k = CreateInstance<KeyPair>();
        k.Main = Main;
        k.Alt = Alt;
        return k;
    }
    public string Name;
    [SerializeField]
    KeyCode main;
    [System.NonSerialized]
    public KeyCode Main;
    [SerializeField]
    KeyCode alt;
    [System.NonSerialized]
    public KeyCode Alt;
    public bool JustPressed => (Main != KeyCode.None && InputManager.GetKeyDown(Main)) || (Alt != KeyCode.None && InputManager.GetKeyDown(Alt));
    public bool JustReleased => (Main != KeyCode.None && InputManager.GetKeyUp(Main)) || (Alt != KeyCode.None && InputManager.GetKeyUp(Alt));
    public bool Pressed => (Main != KeyCode.None && InputManager.GetKey(Main)) || (Alt != KeyCode.None && InputManager.GetKey(Alt));

    public override string ToString() => Main != KeyCode.None ? InputManager.GetKeyName(Main) : Alt != KeyCode.None ? InputManager.GetKeyName(Alt) : "[Not Set]";
}
