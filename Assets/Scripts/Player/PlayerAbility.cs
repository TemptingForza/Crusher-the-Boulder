using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAbility", menuName = "ScriptableObjects/PlayerAbility", order = 1)]
public class PlayerAbility : ScriptableObject
{
    public string Id;
    public string Name;
    [SerializeField]
    bool unlocked;
    [System.NonSerialized]
    public bool Unlocked;
}
