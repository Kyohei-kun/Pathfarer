using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CS_GolbalVolumePilote : MonoBehaviour
{
    Volume volume;

    private bool goToDark;
    [SerializeField] float timeToFade;
    float currentTimeToFade;
    float startTime;
    float startWeight;

    private void Start()
    {
        volume = GetComponent<Volume>();
        this.enabled = false;
        CS_TriggerMerger.VolumePilot = this;
    }

    private void Update()
    {
        if (Time.time < startTime + timeToFade)
        {
            float alpha;
            if (goToDark)
            {
                alpha = Time.time.Remap(startTime, startTime + currentTimeToFade, startWeight, 1);
            }
            else
            {
                alpha = Time.time.Remap(startTime, startTime + currentTimeToFade, startWeight, 0);
            }

            alpha = Mathf.Clamp01(alpha);
            volume.weight = alpha;
        }
        else
        {
            volume.weight = Mathf.RoundToInt(volume.weight);
            this.enabled = false;
        }
    }

    public void FadeToDarkProfil()
    {
        goToDark = true;
        startTime = Time.time;
        this.enabled = true;
        startWeight = volume.weight;
        UpdateStartValues();
    }

    public void FadeToStandardProfil()
    {
        goToDark = false;
        startTime = Time.time;
        this.enabled = true;
        startWeight = volume.weight;
        UpdateStartValues();
    }

    private void UpdateStartValues()
    {
      if (goToDark)
        {
            currentTimeToFade = timeToFade * (1 - volume.weight);
        }
        else
        {
            currentTimeToFade = timeToFade * volume.weight;
        }
    }
}
