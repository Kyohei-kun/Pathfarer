using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_CapaAttack : CS_Pickable
{
    public override void PickEffect()
    {
        base.PickEffect();

        if (!scriptFeatures.State_Attack)
        {
            scriptFeatures.State_Attack = true;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} pris sans nécessité : State_Attack = {scriptFeatures.State_Attack}.");
        }
    }
}
