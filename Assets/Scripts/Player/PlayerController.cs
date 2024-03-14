using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerController : Singleton<PlayerController>, IClimbReciever, IDamageable
{
    public KeyPair inputForward;
    public KeyPair inputBackward;
    public KeyPair inputLeft;
    public KeyPair inputRight;
    public KeyPair inputJump;
    public KeyPair inputSprint;
    public KeyPair inputStop;
    public KeyPair inputAttack;
    public KeyPair inputPowerup;
    public float StoppingForce = 30;
    [SerializeField]
    float movementAccelSpeed;
    public float MovementAccelSpeed => movementAccelSpeed * (CanSprint && (Settings.Instance.toggleSprint ? sprinting : inputSprint.Pressed) ? 1.5f : 1);
    [SerializeField]
    float maxSpeed = 10;
    public float MaxSpeed => maxSpeed * (CanSprint && (Settings.Instance.toggleSprint ? sprinting : inputSprint.Pressed) ? 2f : 1);
    public float JumpStrength = 10;
    public float MaxHealth = 100;
    public float Health = 100;
    public float DamageCooldown = 0.2f;
    public Transform bladeTransform;
    float _dmgCd;
    public Collider bladeCollider;
    public PlayerInteractionHandler interactionHandler;
    bool attacking;
    public Dictionary<Collider, List<Vector3>> collisions = new Dictionary<Collider, List<Vector3>>();
    public Dictionary<Collider,Ref<int>> climbSurfaces = new Dictionary<Collider, Ref<int>>();
    public float LowGravTime;
    public float ClimbingTime;
    bool isClimbing = false;
    public bool CanAttack => ProgressManager.Instance.IsAbilityUnlocked("blades");
    public bool CanSprint => ProgressManager.Instance.IsAbilityUnlocked("sprint");
    public bool CanResize => ProgressManager.Instance.IsAbilityUnlocked("resize");
    public PowerUp heldPowerup;
    public bool sprinting;
    public bool waitingToJump;

    protected void Awake()
    {
        inputForward = InputManager.GetOrCreateButton("moveForward", KeyCode.W);
        inputBackward = InputManager.GetOrCreateButton("moveBackward", KeyCode.S);
        inputLeft = InputManager.GetOrCreateButton("moveLeft", KeyCode.A);
        inputRight = InputManager.GetOrCreateButton("moveRight", KeyCode.D);
        inputJump = InputManager.GetOrCreateButton("moveJump", KeyCode.Space);
        inputSprint = InputManager.GetOrCreateButton("moveSprint", KeyCode.LeftShift);
        inputStop = InputManager.GetOrCreateButton("moveStop", KeyCode.Q);
        inputAttack = InputManager.GetOrCreateButton("attack", KeyCode.R);
        inputPowerup = InputManager.GetOrCreateButton("powerup", KeyCode.X);
        AppearanceManager.Instance.SelectedAppearance.OnApply(this);
    }

    Rigidbody _b;
    public Rigidbody body => _b ?? (_b = GetComponent<Rigidbody>());

    protected void Update()
    {
        if (Settings.Instance.toggleSprint)
            if ((inputForward.Pressed || inputBackward.Pressed || inputLeft.Pressed || inputRight.Pressed) ? inputSprint.JustPressed : sprinting)
                sprinting = !sprinting;
        if (_dmgCd < DamageCooldown)
            _dmgCd += Time.deltaTime;
        if (inputStop.Pressed)
            body.angularVelocity = body.angularVelocity.MoveTowards(Vector3.zero, StoppingForce * Time.deltaTime, out _);
        if (inputJump.JustPressed)
            waitingToJump = true;
        if (inputJump.JustReleased)
            waitingToJump = false;
        Vector3? jump = null;
        if (waitingToJump && collisions.Count > 0)
        {
            var pos = transform.position;
            var offset = Vector3.zero;
            foreach (var c in collisions.Keys.ToArray())
                if (!c)
                    collisions.Remove(c);
            foreach (var c in collisions)
                foreach (var p in c.Value)
                    offset += pos - p;
            if (offset != Vector3.zero)
            {
                if (!Settings.Instance.autoJump)
                    waitingToJump = false;
                body.velocity = body.velocity.LerpDirection(offset.normalized, JumpStrength, 1);
                jump = offset.normalized;
            }
        }
        var flag = true;
        if (ClimbingTime > 0)
        {
            ClimbingTime -= Time.deltaTime;
            if (isClimbing)
            {
                var offset = Vector3.zero;
                var pos = transform.position;
                var nearest = float.PositiveInfinity;
                foreach (var c in climbSurfaces.Keys.ToArray())
                    if (!c)
                        climbSurfaces.Remove(c);
                foreach (var c in climbSurfaces)
                {
                    var p = c.Key.ClosestPoint(pos) - pos;
                    var m = p.sqrMagnitude;
                    if (m <= nearest)
                    {
                        if (m < nearest)
                        {
                            offset = Vector3.zero;
                            nearest = m;
                        }
                        offset += p;
                    }
                }
                if (offset != Vector3.zero)
                {
                    flag = false;
                    var a = Vector3.Angle(PlayerCamera.Instance.cam.transform.forward, offset);
                    var f = Quaternion.LookRotation(PlayerCamera.Instance.cam.transform.forward + ((a > 45 && a < 135) ? Vector3.zero : PlayerCamera.Instance.cam.transform.up), -offset) * Vector3.right;
                    var speed = Mathf.Max(body.angularVelocity.magnitude,MaxSpeed);
                    var mov = Vector3.zero;
                    if (inputLeft.Pressed != inputRight.Pressed)
                        mov += Quaternion.LookRotation(f, -offset) * Vector3.left * (inputLeft.Pressed ? 1 : -1);
                    if (inputForward.Pressed != inputBackward.Pressed)
                        mov +=  f * (inputForward.Pressed ? 1 : -1);
                    if (mov != Vector3.zero)
                    {
                        mov = mov.normalized * MovementAccelSpeed * Time.deltaTime + body.angularVelocity;
                        var newSpeed = mov.magnitude;
                        if (newSpeed > speed)
                            mov = mov / newSpeed * speed;
                        body.angularVelocity = mov;
                    }
                    body.AddForce(offset.normalized * Physics.gravity.magnitude * Time.deltaTime, ForceMode.VelocityChange);
                }
            }
        }
        if (LowGravTime > 0)
            LowGravTime -= Time.deltaTime;
        if (flag)
        {
            var speed = Mathf.Max(body.angularVelocity.magnitude, MaxSpeed);
            var mov = Vector3.zero;
            if (inputLeft.Pressed != inputRight.Pressed)
                mov += PlayerCamera.Instance.transform.forward * (inputLeft.Pressed ? 1 : -1);
            if (inputForward.Pressed != inputBackward.Pressed)
                mov += PlayerCamera.Instance.transform.right * (inputForward.Pressed ? 1 : -1);
            if (mov != Vector3.zero)
            {
                mov = mov.normalized * MovementAccelSpeed * Time.deltaTime + body.angularVelocity;
                var newSpeed = mov.magnitude;
                if (newSpeed > speed)
                    mov = mov / newSpeed * speed;
                body.angularVelocity = mov;
            }
            if (LowGravTime > 0)
                body.AddForce(Physics.gravity * Time.deltaTime * 0.2f, ForceMode.VelocityChange);
            else
                body.AddForce(Physics.gravity * Time.deltaTime, ForceMode.VelocityChange);
        }
            if ((climbSurfaces.Count > 0 && ClimbingTime > 0) != isClimbing)
            isClimbing = !isClimbing;
        if ((attacking && !CanAttack) || (Settings.Instance.toggleBlade ? inputAttack.JustPressed && CanAttack : (inputAttack.Pressed != attacking && CanAttack)))
        {
            attacking = !attacking;
            bladeCollider.enabled = attacking;
        }
        if (inputPowerup.JustPressed && heldPowerup)
        {
            heldPowerup.OnActivate();
            heldPowerup = null;
        }
        AppearanceManager.Instance.SelectedAppearance.OnUpdate(this, new AppearanceParams() { CanClimb = ClimbingTime > 0, IsClimbing = isClimbing, OnGround = collisions.Count > 0, Attacking = attacking, Jumped = jump, InteractionTarget = interactionHandler.lastSelected });
    }

    private void OnCollisionEnter(Collision collision) => collisions[collision.collider] = new List<Vector3>();
    private void OnCollisionStay(Collision collision) {
        collisions[collision.collider].Clear();
        for (int i = 0; i < collision.contactCount; i++)
            collisions[collision.collider].Add(collision.GetContact(i).point);
    }
    private void OnCollisionExit(Collision collision) => collisions.Remove(collision.collider);

    void IClimbReciever.OnEnter(Collider other)
    {
        if (climbSurfaces.TryGetValue(other, out var r))
            r.value++;
        else
            climbSurfaces.Add(other,1);
    }
    void IClimbReciever.OnExit(Collider other)
    {
        if (climbSurfaces.TryGetValue(other, out var r))
        {
            r--;
            if (r <= 0)
                climbSurfaces.Remove(other);
        }
    }

    bool IDamageable.CanDamage(string source) => _dmgCd >= DamageCooldown;

    bool IDamageable.OnDamage(float amount, string source)
    {
        _dmgCd = 0;
        Health -= amount;
        return Health <= 0;
    }
    void IDamageable.Kill()
    {
        foreach (var i in GetComponentsInChildren<IPlayerDeathReciever>())
            i.OnDeath();
        AppearanceManager.Instance.SelectedAppearance.OnDeath(this);
        Destroy(gameObject);
    }
}

public interface IPlayerDeathReciever
{
    void OnDeath();
}
