using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CS_TriggerMerger
{
   static List<CS_TriggerLight> triggerLights = new List<CS_TriggerLight>();

    static bool playerIn;

    public static void AddTrigger(CS_TriggerLight newTrigger)
    {
        triggerLights.Add(newTrigger);
    }

    public static void UpdateMerger()
    {
        playerIn = false;
        foreach (var item in triggerLights)
        {
            if(item.PlayerIN)
            {
                playerIn = true;
                break;
            }
        }
    }
}
