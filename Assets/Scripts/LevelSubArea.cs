using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSubArea : MonoBehaviour
{
    int contains;
    public bool ContainsPlayer => contains > 0;
    public void Update()
    {
        contains--;
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>())
            contains = 2;
    }
}
