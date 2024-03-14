using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleButton : TriggerButton, IInteractable
{

    public virtual bool CanInteract() => !Pressed && enabled;
    public virtual void StateChanged(bool newState) { }
    public virtual void OnInteract() 
    {
        Pressed = true;
    }
}