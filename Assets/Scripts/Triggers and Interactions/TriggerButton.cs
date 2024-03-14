using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerButton : MonoBehaviour
{
    public MonoBehaviour[] triggers;
    bool _pressed;
    public string activateSound;
    public string deactivateSound;
    public bool Pressed
    {
        get => _pressed;
        protected set
        {
            if (_pressed == value)
                return;
            _pressed = value;
            OnPressedChanged();
        }
    }

    protected virtual void OnPressedChanged()
    {
        if (Pressed && activateSound != null)
            AudioManager.Instance.PlayOneShot(activateSound, transform.position);
        else if (!Pressed && deactivateSound != null)
            AudioManager.Instance.PlayOneShot(deactivateSound, transform.position);
        if (triggers != null)
            foreach (var obj in triggers)
                if (obj is IButtonTriggerable trigger)
                    trigger.Trigger(Pressed);
    }
}

public interface IButtonTriggerable
{
    void Trigger(bool state);
}
