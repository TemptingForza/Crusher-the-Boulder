using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSkinOnTrigger : MonoBehaviour, IButtonTriggerable
{
    public Appearance skin;
    void IButtonTriggerable.Trigger(bool state)
    {
        if (state && AppearanceManager.Instance.Appearances.TryGetValue(skin.name, out var value))
        {
            if (value.Unlocked)
                HUDManager.Instance.ShowInformation("You already have the " + value.Name + " skin");
            else
            {
                value.Unlocked = true;
                HUDManager.Instance.ShowInformation("Skin Unlocked: " + value.Name,priority: true);
                ProgressManager.Instance.WriteDataToFile();
            }
        }
    }
}
