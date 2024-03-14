using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitchWeightButton : WeightButton
{
    public Renderer TargetRenderer;
    public Material PressedMaterial;
    public Material ReadyMaterial;
    Material mat
    {
        set
        {
            if (!releasedMat)
                releasedMat = TargetRenderer.material;
            if (value != null && value != TargetRenderer.sharedMaterial)
                TargetRenderer.sharedMaterial = value;
        }
    }
    Material releasedMat;
    protected override void PressedUpdate(float percent)
    {
        base.PressedUpdate(percent);
        mat = Pressed && PressedMaterial ? PressedMaterial : percent > 0 && ReadyMaterial ? ReadyMaterial : releasedMat;
    }
}
