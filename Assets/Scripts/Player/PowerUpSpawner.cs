using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpSpawner : MonoBehaviour, IInteractable
{
    public static PowerUpSpawner ready;
    public PowerUp powerUp;
    public Image image;

    void Awake()
    {
        image.sprite = powerUp.Image;
    }
    bool IInteractable.CanInteract() => PlayerController.Instance.heldPowerup != powerUp;
    void IInteractable.OnInteract() => PlayerController.Instance.heldPowerup = powerUp;
    void IInteractable.StateChanged(bool readyToInteract)
    {
        if (Settings.Instance.autoPowerup && !PlayerController.Instance.heldPowerup && readyToInteract)
            PlayerController.Instance.heldPowerup = powerUp;
        if (readyToInteract)
            ready = this;
        else if (ready == this)
            ready = null;
    }
}
