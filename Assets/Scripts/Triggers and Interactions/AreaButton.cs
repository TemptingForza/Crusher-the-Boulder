using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaButton : TriggerButton
{
    [SerializeField]
    [Tooltip("Only objects on layers provided will be affected. Leave empty to ignore object layer. (Note: the collision and trigger detection is still based on the physics settings)")]
    int[] acceptLayers;
    public HashSet<int> AcceptLayers = new HashSet<int>();
    public TriggerMode Mode;
    int count = 0;
    bool triggeredOnce = false;
    protected virtual void Awake()
    {
        foreach (var layer in acceptLayers)
            AcceptLayers.Add(layer);
    }
    protected virtual void FixedUpdate()
    {
        if (Mode == TriggerMode.Normal || !triggeredOnce || (Mode == TriggerMode.StayOff && Pressed))
            Pressed = count > 0;
        count = 0;
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if (AcceptLayers != null && AcceptLayers.Count > 0 && !AcceptLayers.Contains(other.gameObject.layer))
            return;
        count++;
    }
    protected override void OnPressedChanged()
    {
        base.OnPressedChanged();
        if (Pressed)
            triggeredOnce = true;
    }

    public enum TriggerMode
    {
        Normal,
        StayOn,
        StayOff
    }
}
