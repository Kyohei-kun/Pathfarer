using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public static class CS_ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


    public static float Lenght(this NavMeshPath path)
    {
        if(path.corners.Count() < 2) return -1;
        float result = 0;

        for (int i = 1; i < path.corners.Count(); i++)
        {
            result += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return result;
    }
}
