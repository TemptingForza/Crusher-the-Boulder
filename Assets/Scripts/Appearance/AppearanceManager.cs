using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceManager : Singleton<AppearanceManager>
{
    [SerializeField]
    List<Appearance> appearances;
    public Dictionary<string, Appearance> Appearances = new Dictionary<string, Appearance>();
    [SerializeField]
    int initialAppearanceIndex = 0;
    string selected;
    protected void Awake()
    {
        foreach (var a in appearances)
            Appearances.Add(a.name, a);
        selected = appearances[initialAppearanceIndex].name;
    }
    public string SelectedAppearanceName
    {
        get => selected;
        set
        {
            if (Appearances.TryGetValue(value, out var a))
            {
                OnBeforeChange();
                selected = value;
                OnChange();
            }
            else
                Debug.LogWarning("[AppearanceManager.SelectedAppearanceName] Appearance \"" + value + "\" not found");
        }
    }
    public Appearance SelectedAppearance => Appearances[SelectedAppearanceName];
    void OnBeforeChange()
    {
        if (PlayerController.Instance)
            SelectedAppearance.OnRemove(PlayerController.Instance);
    }
    void OnChange()
    {
        if (PlayerController.Instance)
            SelectedAppearance.OnApply(PlayerController.Instance);
    }
}