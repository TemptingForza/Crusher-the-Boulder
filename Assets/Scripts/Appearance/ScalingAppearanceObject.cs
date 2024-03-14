using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScalingAppearanceObject : AppearanceObject
{
    public Vector3 modelTransition = new Vector3(0.8f, 1, 0.5f);
    public bool hideOnMinSize = true;
    public void Start()
    {
        transform.localScale = Vector3.one * modelTransition.x;
    }
    public override void OnUpdate(AppearanceParams appearanceParams)
    {
        base.OnUpdate(appearanceParams);
        if (ScaleUp(appearanceParams))
            transform.localScale = Vector3.one * Mathf.MoveTowards(transform.localScale.x, modelTransition.y, Time.deltaTime / modelTransition.z);
        else
            transform.localScale = Vector3.one * Mathf.MoveTowards(transform.localScale.x, modelTransition.x, Time.deltaTime / modelTransition.z);
        if (hideOnMinSize && (gameObject.activeSelf == (transform.localScale.x == modelTransition.x)))
            gameObject.SetActive(transform.localScale.x != modelTransition.x);
    }
    protected abstract bool ScaleUp(AppearanceParams appearanceParams);
}
