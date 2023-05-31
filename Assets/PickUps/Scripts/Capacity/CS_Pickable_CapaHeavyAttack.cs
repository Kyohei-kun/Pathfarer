using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_CapaHeavyAttack : CS_Pickable
{
    public override void PickEffect()
    {
        base.PickEffect();

        if (!scriptFeatures.State_HeavyAttack)
        {
            scriptFeatures.State_HeavyAttack = true;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} pris sans nécessité : State_HeavyAttack = {scriptFeatures.State_HeavyAttack}.");
        }
    }
}
