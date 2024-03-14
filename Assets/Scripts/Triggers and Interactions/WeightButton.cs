using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightButton : TriggerButton
{
    public float requiredForce = 1;
    public float maxOffset = 0.2f;
    public bool stayDown;
    HashSet<Rigidbody> bodies = new HashSet<Rigidbody>();
    float pushForce;
    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.isTrigger || !other.attachedRigidbody)
            return;
        bodies.Add(other.attachedRigidbody);
    }
    protected virtual void FixedUpdate()
    {
        pushForce = 0;
        foreach (var body in bodies)
            pushForce += body.mass;
        bodies.Clear();
    }
    protected virtual void Update()
    {
        if (pushForce >= requiredForce || (Pressed && stayDown))
        {
            Pressed = true;
            PressedUpdate(1);
        } else
        {
            Pressed = false;
            PressedUpdate(pushForce / requiredForce);
        }
    }
    Vector3 initialPos;
    protected virtual void Start()
    {
        initialPos = transform.localPosition;
    }
    protected virtual void PressedUpdate(float percent)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, initialPos + transform.InverseTransformDirection(-transform.up) * maxOffset * percent,Time.deltaTime);
    }
}
