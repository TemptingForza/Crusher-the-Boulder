using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingAppearanceObject : ScalingAppearanceObject
{
    protected override bool ScaleUp(AppearanceParams appearanceParams) => appearanceParams.CanClimb;
}
