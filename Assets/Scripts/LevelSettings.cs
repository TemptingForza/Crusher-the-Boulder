using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "ScriptableObjects/LevelSettings", order = 1)]
public class LevelSettings : ScriptableObject, IComparable<LevelSettings>
{
    public string Id;
    public string Name;
    public GameObject Prefab;
    [SerializeField]
    bool unlocked;
    [NonSerialized]
    public bool Unlocked;
    public Sprite Image;
    public bool RestartOnDeath;
    public int SortOrder;
    protected virtual void Awake()
    {
        Unlocked = unlocked;
    }
    public int CompareTo(LevelSettings value) => comparer.Compare(this, value);
    public static IComparer<LevelSettings> comparer = new LevelComparer();
    class LevelComparer : IComparer<LevelSettings>
    {
        int IComparer<LevelSettings>.Compare(LevelSettings x, LevelSettings y) => x.SortOrder.CompareTo(y.SortOrder);
    }
}
