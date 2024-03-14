using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveOnTrigger : MonoBehaviour, IButtonTriggerable
{
    public float timeToMove;
    public Vector3 offset;
    [NonSerialized]
    public Vector3 start;
    bool moving;
    float mov;
    void IButtonTriggerable.Trigger(bool state) => moving = state;
    void Start()
    {
        start = transform.localPosition;
    }
    void Update()
    {
        if (moving && mov < timeToMove)
            transform.localPosition = Vector3.Lerp(start, start + offset, (mov = Mathf.MoveTowards(mov, timeToMove, Time.deltaTime)) / timeToMove);
        if (!moving && mov > timeToMove)
            transform.localPosition = Vector3.Lerp(start, start + offset, (mov = Mathf.MoveTowards(mov, 0, Time.deltaTime)) / timeToMove);
    }
}
