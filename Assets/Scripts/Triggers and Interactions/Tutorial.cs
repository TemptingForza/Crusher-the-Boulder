using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public KeyPair forward;
    public KeyPair backward;
    public KeyPair left;
    public KeyPair right;
    public KeyPair jump;
    public KeyPair stop;
    void Start()
    {
        HUDManager.Instance.ShowInformation($"Use {forward}, {left}, {backward} and {right} to roll around", _ => forward.JustPressed || left.JustPressed || backward.JustPressed || right.JustPressed);
        HUDManager.Instance.ShowInformation($"Use {stop} to stop rolling", _ => stop.JustPressed);
        HUDManager.Instance.ShowInformation($"Use {jump} to jump", _ => jump.JustPressed);
        HUDManager.Instance.ShowInformation($"Get rolling");
    }
}
