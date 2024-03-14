using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEffectPowerUp", menuName = "ScriptableObjects/PlayerEffectPowerUp", order = 1)]
public class PlayerEffectPowerUp : PowerUp
{
    public PlayerEffect effect;
    public override void OnActivate()
    {
        if (effect == PlayerEffect.Climb)
            PlayerController.Instance.ClimbingTime = 10;
        else if (effect == PlayerEffect.LowGrav)
            PlayerController.Instance.LowGravTime = 10;
    }
}

public enum PlayerEffect
{
    Climb,
    LowGrav
}
