using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public Vector3 spin = Vector3.up * 30;
    public void Update()
    {
        transform.localRotation *= Quaternion.Euler(spin * Time.deltaTime);
    }
}
