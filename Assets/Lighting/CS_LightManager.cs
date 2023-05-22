using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CS_LightManager : MonoBehaviour
{
    [BoxGroup("DirectionnalLight")][SerializeField] Light directionnalLight;
    [BoxGroup("DirectionnalLight")][SerializeField] float highDirectionnalTarget;

    [BoxGroup("FireFlies")][SerializeField] Light fireFliesLight;
    [BoxGroup("FireFlies")][ReadOnly][SerializeField] float highFirefliesTarget;

    [BoxGroup("PlayerLight")][SerializeField] Light playerLight;
    [BoxGroup("PlayerLight")][ReadOnly][SerializeField] float highPlayerTarget;

    [BoxGroup("MentorLight")][SerializeField] Light mentorLight;
    [BoxGroup("MentorLight")][ReadOnly][SerializeField] float highMentorTarget;

    CS_EntriesLights entriesLights;

    private bool goHigh;
    [SerializeField] float timeToFade = 5;
    float currentTimeToFade;
    float startTime;
    float startAlpha;
    [ProgressBar("Alpha", 1, EColor.Red)][SerializeField] float globalAlpha = 0;


    private void Start()
    {
        try
        {
            entriesLights = GameObject.FindGameObjectWithTag("EntriesLights").GetComponent<CS_EntriesLights>();
        }
        catch (System.Exception)
        {
            Debug.LogError("Pas de entries light dans les scenes chargées");
        }

        CS_TriggerMerger.LightManager = this;

        this.enabled = false;

        highDirectionnalTarget = directionnalLight.intensity;
        highFirefliesTarget = fireFliesLight.intensity;
        highMentorTarget = mentorLight.intensity;
        highPlayerTarget = playerLight.intensity;

        fireFliesLight.intensity = 0;
        mentorLight.intensity = 0;
        playerLight.intensity = 0;
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

        if(entriesLights != null) entriesLights.UpdateLight(globalAlpha);
    }

    [Button]
    public void FadeToDarkProfil()
    {
        if (globalAlpha >= 1)
            return;
        goHigh = true;
        startTime = Time.time;
        this.enabled = true;
        startAlpha = globalAlpha;
        UpdateStartAlpha();
    }

    [Button]
    public void FadeToStandardProfil()
    {
        if (globalAlpha <= 0) return;
        goHigh = false;
        startTime = Time.time;
        this.enabled = true;
        startAlpha = globalAlpha;
        UpdateStartAlpha();
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
        float oneMinusAlpha = Mathf.Clamp01(1 - globalAlpha);
        directionnalLight.intensity = Mathf.Lerp(0, highDirectionnalTarget, oneMinusAlpha);
        fireFliesLight.intensity = Mathf.Lerp(0, highFirefliesTarget, globalAlpha);
        mentorLight.intensity = Mathf.Lerp(0, highMentorTarget, globalAlpha);
        playerLight.intensity = Mathf.Lerp(0, highPlayerTarget, globalAlpha);
    }

}
