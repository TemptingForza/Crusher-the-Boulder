using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitchButton : SimpleButton
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
            if (value != null)
                TargetRenderer.sharedMaterial = value;
        }
    }
    Material releasedMat;
    public override void StateChanged(bool newState)
    {
        base.StateChanged(newState);
        if (newState)
            mat = ReadyMaterial;
        else
            mat = Pressed && PressedMaterial ? PressedMaterial : releasedMat;
    }
    protected override void OnPressedChanged()
    {
        base.OnPressedChanged();
        mat = Pressed && PressedMaterial ? PressedMaterial : releasedMat;
    }
}
