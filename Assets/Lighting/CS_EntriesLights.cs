using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class CS_EntriesLights: MonoBehaviour
{
    Dictionary<Light, float> lights = new Dictionary<Light, float>();

    private void Start()
    {
        foreach (var newLight in GetComponentsInChildren<Light>())
        {
            lights.Add(newLight, newLight.intensity);
            newLight.intensity = 0;
        }
    }

    public void UpdateLight(float alpha)
    {
        foreach (var light in lights)
        {
            light.Key.intensity = Mathf.Lerp(0, light.Value, alpha);
        }
    }
}
