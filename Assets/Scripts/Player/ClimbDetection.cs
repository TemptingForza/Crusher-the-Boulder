using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbDetection : MonoBehaviour
{
    IClimbReciever _r;
    public IClimbReciever reciever
    {
        get
        {
            if (_r == null)
                _r = GetComponentInParent<IClimbReciever>();
            return _r;
        }
    }
    void OnTriggerEnter(Collider other) => reciever.OnEnter(other);
    void OnTriggerExit(Collider other) => reciever.OnExit(other);
}

public interface IClimbReciever
{
    void OnEnter(Collider other);
    void OnExit(Collider other);
}
