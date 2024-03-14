using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTutorial : MonoBehaviour
{
    public KeyPair interact;
    bool entered = true;
    bool showing = false;
    void FixedUpdate()
    {
        if (entered)
        {
            if (!showing)
            {
                showing = true;
                HUDManager.Instance.ShowInformation("This is a spawn point. You can press " + interact + " to activate it. When you die, or use the respawn option in the pause menu, you'll be taken back to the last spawn point you activated", x => !showing);
            }
        }
        else
            showing = false;
        entered = false;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>())
            entered = true;
    }
}
