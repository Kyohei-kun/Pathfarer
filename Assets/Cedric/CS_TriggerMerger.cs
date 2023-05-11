using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CS_TriggerMerger
{
    static List<CS_TriggerLight> triggerLights = new List<CS_TriggerLight>();
    static bool playerIn;
    static bool lastPlayerIn;
    static private CS_LightManager lightManager;

    public static CS_LightManager LightManager { get => lightManager; set => lightManager = value; }

    public static void AddTrigger(CS_TriggerLight newTrigger)
    {
        triggerLights.Add(newTrigger);
    }

    public static void UpdateMerger()
    {
        playerIn = false;
        foreach (var item in triggerLights)
        {
            if (item.PlayerIN)
            {
                playerIn = true;
                break;
            }
        }

        if(lastPlayerIn != playerIn)
        {
            lightManager.ChangeSate(playerIn);
        }

        lastPlayerIn = playerIn;
    }
}
