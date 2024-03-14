using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MaterialSwitchButton
{
    bool ready;
    public override void StateChanged(bool newState)
    {
        base.StateChanged(newState);
        ready = newState;
        if (newState)
            HUDManager.Instance.ShowInformation("Use " + PlayerController.Instance.interactionHandler.inputInteract + " to activate sigils",x => !ready);
    }
}
