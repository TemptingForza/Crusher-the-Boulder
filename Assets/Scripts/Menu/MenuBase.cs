using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuBase : MonoBehaviour
{
    public MenuItem menuItemPrefab;
    public virtual void AddMenuItem(string label,Action onClick)
    {
        var g = Instantiate(menuItemPrefab, transform, false);
        g.SetText(label);
        g.OnClick = onClick;
    }
    public virtual void ClearMenuItems(bool immediate = false)
    {
        foreach (Transform t in transform)
            if (immediate)
                DestroyImmediate(t.gameObject);
            else
                Destroy(t.gameObject);
    }
    protected virtual void OnEnable()
    {
        AudioManager.Instance.PlayOneShot("click");
    }
    protected virtual void OnDisable()
    {
        AudioManager.Instance.PlayOneShot("click");
    }
}
