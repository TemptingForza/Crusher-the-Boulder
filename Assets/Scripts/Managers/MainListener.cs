using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainListener : Singleton<MainListener>
{
    public Vector3 listenPos;
    public AudioListener listener;
    void Update() => listenPos = listener.transform.position + listener.transform.forward;
    void FixedUpdate() => Update();
}
