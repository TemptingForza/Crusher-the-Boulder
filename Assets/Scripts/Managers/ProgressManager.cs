using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;
using System.Json;
using System.IO;

public class ProgressManager : SavedSingleton<ProgressManager>
{
    public Dictionary<string, LevelSettings> Levels = new Dictionary<string, LevelSettings>();
    public Dictionary<string, PlayerAbility> Abilities = new Dictionary<string, PlayerAbility>();
    HashSet<string> tempAbilities = new HashSet<string>();
    [SerializeField]
    LevelSettings[] levels;
    [SerializeField]
    PlayerAbility[] abilities;
    protected void Awake()
    {
        fileName = Path.Combine(Application.persistentDataPath, "progress.json");
        foreach (var l in levels)
            Levels.Add(l.Id, l);
        foreach (var a in abilities)
            Abilities.Add(a.Id, a);
        HUDManager.getAbilityInformation += GetAbilityInfo;
    }
    static string GetAbilityInfo(PlayerAbility ability)
    {
        if (ability.Id == "blades")
            return "<b>Ability: " + ability.Name + "</b>\nPress " + InputManager.GetButton("attack") + " to use this ability. Can be used to damage certain object or fight enemies";
        if (ability.Id == "resize")
            return "<b>Ability: " + ability.Name + "</b>\nPress " + InputManager.GetButton("resize") + " to use this ability. Makes you bigger and heavier";
        if (ability.Id == "sprint")
            return "<b>Ability: " + ability.Name + "</b>\nPress " + InputManager.GetButton("moveSprint") + " to use this ability. Increases your movement speed";
        return null;
    }
    protected override void ReadData(JsonObject savedData)
    {
        if (savedData.TryGetValue("levelUnlocks", out var l) && l is JsonObject levelUnlocks)
            foreach (var unlock in levelUnlocks)
                if (Levels.TryGetValue(unlock.Key, out var level))
                    levelUnlocks.TryGetValue(unlock.Key, ref level.Unlocked);
        if (savedData.TryGetValue("skinUnlocks", out var s) && s is JsonObject skinUnlocks)
            foreach (var unlock in skinUnlocks)
                if (AppearanceManager.Instance.Appearances.TryGetValue(unlock.Key, out var appearance))
                    skinUnlocks.TryGetValue(unlock.Key, ref appearance.Unlocked);
        if (savedData.TryGetValue2("selectedSkin", out string n) && AppearanceManager.Instance.Appearances.TryGetValue(n,out var activeAppearance) && activeAppearance.Unlocked)
            AppearanceManager.Instance.SelectedAppearanceName = n;
        if (savedData.TryGetValue("abilityUnlocks", out var a) && a is JsonObject abilityUnlocks)
            foreach (var unlock in abilityUnlocks)
                if (Abilities.TryGetValue(unlock.Key, out var ability))
                    abilityUnlocks.TryGetValue(unlock.Key, ref ability.Unlocked);
    }

    protected override JsonObject WriteData()
    {
        var data = new JsonObject();
        var levelUnlocks = data.CreateChild("levelUnlocks");
        foreach (var l in Levels)
            levelUnlocks[l.Key] = l.Value.Unlocked;
        var skinUnlocks = data.CreateChild("skinUnlocks");
        foreach (var s in AppearanceManager.Instance.Appearances)
            skinUnlocks[s.Key] = s.Value.Unlocked;
        data.Add("selectedSkin", AppearanceManager.Instance.SelectedAppearanceName);
        var abilityUnlocks = data.CreateChild("abilityUnlocks");
        foreach (var a in Abilities)
            abilityUnlocks[a.Key] = a.Value.Unlocked;
        return data;
    }

    public bool IsAbilityUnlocked(string id)
    {
        if (tempAbilities.Contains(id))
            return true;
        if (Abilities.TryGetValue(id, out var ability))
            return ability.Unlocked;
        Debug.LogWarning("[ProgressManager.IsAbilityUnlocked] Ability \"" + id + "\" not found");
        return false;
    }

    public bool UnlockAbility(string id, bool temporary = true)
    {
        if (Abilities.TryGetValue(id, out var ability) && !ability.Unlocked)
        {
            if (temporary)
                return tempAbilities.Add(id);
            else
            {
                tempAbilities.Remove(id);
                ability.Unlocked = true;
                return true;
            }
        }
        if (!ability)
            Debug.LogWarning("[ProgressManager.UnlockAbility] Ability \"" + id + "\" not found");
        return false;
    }

    public void RelockTemporaryAbilities() => tempAbilities.Clear();
    public void FullUnlockTemporaryAbilities()
    {
        IEnumerator<string> e; 
        while ((e = tempAbilities.GetEnumerator()).MoveNext())
            UnlockAbility(e.Current, false);
    }
}
