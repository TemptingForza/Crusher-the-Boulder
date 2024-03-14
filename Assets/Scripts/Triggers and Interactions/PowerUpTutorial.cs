using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTutorial : MonoBehaviour
{
    public KeyPair interact;
    public KeyPair powerup;
    bool entered = true;
    bool showing = false;
    void FixedUpdate()
    {
        if (entered)
        {
            if (!showing)
            {
                showing = true;
                HUDManager.Instance.ShowInformation("This is a powerup. " + (Settings.Instance.autoPowerup ? "If you're not holding a powerup and touch it then you'll automatically pick it up. If you're already holding a powerup, y" : "Y") + "ou can press " + interact + " to collect it", x => !showing || (x > 4 && PlayerController.Instance.heldPowerup));
                HUDManager.Instance.ShowInformation("While holding a powerup you can press " + powerup + " to use it. Try using this one to climb the wall over there", x => !PlayerController.Instance.heldPowerup);
            }
        }
        else
        {
            showing = false;
        }
        entered = false;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>())
            entered = true;
    }
}
