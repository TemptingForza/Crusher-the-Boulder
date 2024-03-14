using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Appearance", menuName = "ScriptableObjects/Appearance", order = 1)]
public class Appearance : ScriptableObject
{
    public string Name;
    public Sprite Image;
    public AppearanceObject Body;
    public Material BodyMaterial;
    public GameObject DeathPrefab;
    public AppearanceObject ClimbingPrefab;
    public AppearanceObject BladePrefab;
    public string DeathSound;
    [SerializeField]
    bool unlocked;
    [System.NonSerialized]
    public bool Unlocked;
    protected virtual void Awake()
    {
        Unlocked = unlocked;
    }
    public virtual void OnApply(PlayerController player)
    {
        var body = Instantiate(Body, player.transform);
        if (BodyMaterial)
            body.GetComponent<Renderer>().sharedMaterial = BodyMaterial;
        Instantiate(ClimbingPrefab, player.transform);
        Instantiate(BladePrefab,player.bladeTransform);
    }
    public virtual void OnRemove(PlayerController player)
    {
        foreach (var o in player.GetComponentsInChildren<AppearanceObject>(true))
            Destroy(o.gameObject);
    }
    public virtual void OnUpdate(PlayerController player, AppearanceParams appearanceParams)
    {
        foreach (var o in player.GetComponentsInChildren<AppearanceObject>(true))
            o.OnUpdate(appearanceParams);
    }
    public virtual void OnDeath(PlayerController player)
    {
        if (DeathSound != null)
            AudioManager.Instance.PlayOneShot(DeathSound, player.transform.position);
        var o = Instantiate(DeathPrefab, player.transform.position, player.transform.rotation);
        o.transform.SetParent(LevelManager.Instance?.spawnedLevel?.transform, true);
        foreach (var r in o.GetComponentsInChildren<Rigidbody>())
        {
            r.velocity = player.body.velocity;
            r.angularVelocity = player.body.angularVelocity;
        }
    }
}

public struct AppearanceParams
{
    public bool CanClimb;
    public bool IsClimbing;
    public bool OnGround;
    public bool Attacking;
    public Vector3? Jumped;
    public IInteractable InteractionTarget;
}
