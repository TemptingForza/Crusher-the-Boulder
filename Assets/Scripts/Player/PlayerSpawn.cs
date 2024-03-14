using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour, IInteractable, IButtonTriggerable
{
    public static PlayerSpawn ActiveSpawn;
    public static PlayerSpawn DefaultSpawn;
    public bool CanBeActivated = true;
    [SerializeField]
    bool IsDefault;
    public bool IsActive => this == ActiveSpawn;
    public virtual bool CanInteract() { return enabled && CanBeActivated && !IsActive; }
    public virtual void OnInteract()
    {
        ActiveSpawn = this;
    }
    public virtual void StateChanged(bool readyToInteract) { }

    protected void Awake()
    {
        if (IsDefault)
        {
            if (!DefaultSpawn || !DefaultSpawn.isActiveAndEnabled)
                DefaultSpawn = this;
            if (!ActiveSpawn || !ActiveSpawn.isActiveAndEnabled)
                ActiveSpawn = this;
        }
    }
    protected void OnEnable() => Awake();
    protected void OnDisable() => OnDestroy();

    public virtual void Trigger(bool state)
    {
        if (state)
            OnInteract();
    }

    protected void OnDestroy()
    {
        if (IsActive)
            ActiveSpawn = DefaultSpawn && DefaultSpawn.isActiveAndEnabled ? DefaultSpawn : null;   
    }
}
