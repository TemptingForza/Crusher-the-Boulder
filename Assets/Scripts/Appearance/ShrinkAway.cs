using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkAway : MonoBehaviour
{
    public float waitTime = 1;
    public float transitionTime = 0.5f;
    float time;
    void Update()
    {
        time += Time.deltaTime;
        if (time >= transitionTime + waitTime)
            DestroyImmediate(gameObject);
        else if (time >= waitTime)
            transform.localScale = Vector3.one * (1 - ((time - waitTime) / transitionTime));
    }
}
