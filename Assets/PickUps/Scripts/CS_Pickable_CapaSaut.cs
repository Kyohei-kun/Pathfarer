using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_CapaSaut : CS_Pickable
{
    public override void PickEffect()
    {
        base.PickEffect();

        if (!scriptFeatures.State_Jump)
        {
            scriptFeatures.State_Jump = true;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} pris sans nécessité : State_Jump = {scriptFeatures.State_Jump}.");
        }
    }
}
