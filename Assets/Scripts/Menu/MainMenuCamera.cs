using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MainCamera
{
    protected override void Update()
    {
        base.Update();
        transform.localRotation = Quaternion.LookRotation(MainMenuPlayer.Instance.transform.position - transform.position);
    }
}
