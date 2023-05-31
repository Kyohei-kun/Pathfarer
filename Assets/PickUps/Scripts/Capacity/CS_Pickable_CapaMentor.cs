using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Pickable_CapaMentor : CS_Pickable
{
    public override void PickEffect()
    {
        base.PickEffect();

        if (!scriptFeatures.State_Mentor)
        {
            scriptFeatures.State_Mentor = true;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} pris sans nécessité : State_FireFlies = {scriptFeatures.State_Mentor}.");
        }
    }
}
