using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MainCamera
{
    public float maxCameraDifference = 2;
    public float cameraCastRadius = 0.2f;
    [SerializeField]
    bool takeMouse = true;
    public bool TakeMouse
    {
        get => takeMouse;
        set
        {
            takeMouse = value;
            if (takeMouse)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    public Layer castLayer;
    int _rm = 0;
    int raymask
    {
        get
        {
            if (_rm == 0)
                for (int i = 0; i < 32; i++)
                    if (!Physics.GetIgnoreLayerCollision((int)castLayer, i))
                        _rm |= 1 << i;
            return _rm;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (PlayerController.Instance)
            transform.position = PlayerController.Instance.transform.position;
        if (Cursor.lockState == CursorLockMode.Locked != takeMouse)
            TakeMouse = takeMouse;
        if (TakeMouse)
        {
            transform.localEulerAngles += Vector3.up * Input.GetAxis("Mouse X") * (Settings.Instance.invertMouseX ? -1 : 1) * Settings.Instance.mouseSensitivity * 360 * Time.deltaTime;
            var a = cam.transform.localEulerAngles + Vector3.right * Input.GetAxis("Mouse Y") * (Settings.Instance.invertMouseY ? 1 : -1) * Settings.Instance.mouseSensitivity * 360 * Time.deltaTime;
            if (a.x < 0)
                a.x = 360 + (a.x % 360);
            if (a.x > 360)
                a.x = a.x % 360;
            if (a.x > 90 && a.x <= 180)
                a.x = 90;
            if (a.x > 180 && a.x < 270)
                a.x = 270;
            cam.transform.localEulerAngles = a;
            cam.transform.localPosition = cam.transform.localRotation * -Vector3.forward * (Physics.SphereCast(transform.position, cameraCastRadius, -cam.transform.forward, out var hit, maxCameraDifference, raymask, QueryTriggerInteraction.Ignore) ? hit.distance : maxCameraDifference);
        }
    }
}
