using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_Key : CS_Pickable
{
    public override void PickEffect()
    {
        base.PickEffect();

        scriptFeatures.AddInInventory(name);
    }
}
