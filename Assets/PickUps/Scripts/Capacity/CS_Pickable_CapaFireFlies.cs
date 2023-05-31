using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_CapaFireFlies : CS_Pickable
{
    public override void PickEffect()
    {
        base.PickEffect();

        if (!scriptFeatures.State_FireFlies)
        {
            scriptFeatures.State_FireFlies = true;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} pris sans nécessité : State_FireFlies = {scriptFeatures.State_FireFlies}.");
        }
    }
}
