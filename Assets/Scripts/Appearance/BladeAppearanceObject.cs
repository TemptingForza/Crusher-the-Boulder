using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BladeAppearanceObject : ScalingAppearanceObject
{
    protected override bool ScaleUp(AppearanceParams appearanceParams) => appearanceParams.Attacking;
}
