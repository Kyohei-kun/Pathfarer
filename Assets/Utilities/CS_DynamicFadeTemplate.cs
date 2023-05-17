using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_DynamicFadeTemplate : MonoBehaviour
{
    private bool goHigh;
    [SerializeField] float timeToFade = 5;
    float currentTimeToFade;
    float startTime;
    float startAlpha;
    [ProgressBar("Alpha", 1, EColor.Red)][SerializeField] float globalAlpha = 0;

    [Button]
    public void FadeToDarkProfil()
    {
        goHigh = true;
        startTime = Time.time;
        this.enabled = true;
        startAlpha = globalAlpha;
        UpdateStartAlpha();
    }

    [Button]
    public void FadeToStandardProfil()
    {
        goHigh = false;
        startTime = Time.time;
        this.enabled = true;
        startAlpha = globalAlpha;
        UpdateStartAlpha();
    }

    private void Start()
    {
        this.enabled = false;
    }

    private void Update()
    {
        if (Time.time < startTime + timeToFade)
        {
            float alpha;
            if (goHigh)
            {
                alpha = Time.time.Remap(startTime, startTime + currentTimeToFade, startAlpha, 1);
            }
            else
            {
                alpha = Time.time.Remap(startTime, startTime + currentTimeToFade, startAlpha, 0);
            }

            alpha = Mathf.Clamp01(alpha);
            globalAlpha = alpha;

        }
        else
        {
            globalAlpha = Mathf.RoundToInt(globalAlpha);
            this.enabled = false;
        }

        UpdateObjects();
    }


    private void UpdateStartAlpha()
    {
        if (goHigh)
        {
            currentTimeToFade = timeToFade * (1 - globalAlpha);
        }
        else
        {
            currentTimeToFade = timeToFade * globalAlpha;
        }
    }
    private void UpdateObjects()
    {
        //Update objects by new alpha
    }

}
