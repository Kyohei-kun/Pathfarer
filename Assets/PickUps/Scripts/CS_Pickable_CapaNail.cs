using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_CapaNail : CS_Pickable
{
    public override void PickEffect()
    {
        base.PickEffect();

        if (!scriptFeatures.State_Nail)
        {
            scriptFeatures.State_Nail = true;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} pris sans nécessité : State_Nail = {scriptFeatures.State_Nail}.");
        }
    }
}
