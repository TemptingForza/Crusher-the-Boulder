using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Only objects on layers provided will be affected. Leave empty to ignore object layer. (Note: the collision and trigger detection is still based on the physics settings)")]
    Layer[] layers;
    public HashSet<Layer> Layers = new HashSet<Layer>();
    [Tooltip("Controls the type of colliders that will be detected by the OnTriggerStay event")]
    public ColliderState OnTrigger;
    [Tooltip("Controls the type of colliders that will be detected by the OnCollisionStay event")]
    public ColliderState OnCollide;
    [Tooltip("If enabled, damage amount and source will be ignored and instead this will simply kill any valid targets")]
    public bool InstantKill;
    [Tooltip("In most cases this will have no affect on the damage dealt however damagable objects may be setup to behave differently for a specific damage source")]
    public string DamageSource;
    [Tooltip("The amount of damage to send to targeted objects. Keep in mind, an object is able to determine how much damage it takes, independant of what this is set to, but generally this will be taken into consideration")]
    public float DamageAmount;
    protected virtual void Awake()
    {
        foreach (var layer in layers)
            Layers.Add(layer);
    }
    protected virtual void OnTriggerStay(Collider other) => TryDamage(other, OnTrigger);
    protected virtual void OnCollisionStay(Collision other) => TryDamage(other.collider, OnCollide);

    void TryDamage(Collider collider, ColliderState flags)
    {
        if (flags == ColliderState.None || !(collider.isTrigger ? flags.HasFlag(ColliderState.Trigger) : flags.HasFlag(ColliderState.Collide)))
            return;
        if (Layers == null || Layers.Count == 0 || Layers.Contains((Layer)collider.gameObject.layer))
            foreach (var target in collider.GetComponents<IDamageable>())
                if (InstantKill || (target.CanDamage(DamageSource) && target.OnDamage(DamageAmount, DamageSource)))
                    target.Kill();
    }
}

public enum ColliderState
{
    None,
    Collide,
    Trigger,
    CollideAndTrigger = Collide + Trigger
}