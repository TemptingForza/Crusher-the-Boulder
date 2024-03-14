using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLevelOnTrigger : MonoBehaviour, IButtonTriggerable
{
    public LevelSettings level;
    public bool unlock;
    public bool SaveAbilityUnlocks = true;
    void IButtonTriggerable.Trigger(bool state)
    {
        if (state)
        {
            if (unlock && level && !level.Unlocked)
            {
                level.Unlocked = true;
                if (!SaveAbilityUnlocks)
                    ProgressManager.Instance.WriteDataToFile();
            }
            if (SaveAbilityUnlocks) {
                ProgressManager.Instance.FullUnlockTemporaryAbilities();
                ProgressManager.Instance.WriteDataToFile();
            }
            LevelManager.CurrentLevel = level;
        }
    }
}
