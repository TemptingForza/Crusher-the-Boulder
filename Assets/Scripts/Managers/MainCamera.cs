using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainCamera : Singleton<MainCamera>
{
    public Camera cam;
    protected virtual void Update()
    {
        if (cam.fieldOfView != Settings.Instance.FOV)
            cam.fieldOfView = Settings.Instance.FOV;
    }
}
