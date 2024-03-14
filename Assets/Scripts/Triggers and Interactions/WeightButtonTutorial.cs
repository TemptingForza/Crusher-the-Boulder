using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightButtonTutorial : MonoBehaviour
{
    public WeightButton button;
    bool entered = false;
    bool showing = false;
    void FixedUpdate()
    {
        if (entered && !button.Pressed)
        {
            if (!showing)
            {
                showing = true;
                HUDManager.Instance.ShowInformation("This is a weighted preassure plate. This one you can just roll onto to trigger", x => !showing && x > 3);
            }
        } else
        {
            if (showing)
            {
                showing = false;
                if (button.Pressed)
                    HUDManager.Instance.ShowInformation("This button is staying down, but keep in mind some will require more weight and or will reset when the weight is taken off them",8);
            }
        }
        entered = false;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>())
            entered = true;
    }
}
