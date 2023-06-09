using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickup_CapaNailV2 : CS_Pickable
{
    public override void PickEffect()
    {
        base.PickEffect();

        if (!scriptFeatures.State_Nail_V2)
        {
            scriptFeatures.State_Nail_V2 = true;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} pris sans nécessité : State_Nail_V2 = {scriptFeatures.State_Nail_V2}.");
        }
    }
}
