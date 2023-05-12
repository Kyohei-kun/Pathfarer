using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_LightManager : MonoBehaviour
{
    Light directionnalLight;
    [SerializeField] float speedFade = 2;
    [SerializeField] float standardIntensity = 10000;
    [SerializeField] float darkIntensity = 0;
    [SerializeField] float rate = 1;

    Coroutine currentLerpCoroutine;

    [ReadOnly] public bool PlayerIsInDarkZone;

    private void Start()
    {
        directionnalLight = GetComponent<Light>();
        CS_TriggerMerger.LightManager = this;
    }

    public void ChangeSate(bool playerIn)
    {
        PlayerIsInDarkZone = playerIn;
        if (currentLerpCoroutine != null)
        StopCoroutine(currentLerpCoroutine);

        if (playerIn)
        {
            currentLerpCoroutine = StartCoroutine(LerpToDark());
            //directionnalLight.intensity = 0;
        }
        else
        {
            currentLerpCoroutine = StartCoroutine(LerpToStandard());
            //directionnalLight.intensity = 100000;
        }
    }

    IEnumerator LerpToStandard()
    {
        while (directionnalLight.intensity < standardIntensity)
        {
            directionnalLight.intensity += rate;
            yield return 0;
        }

        directionnalLight.intensity = standardIntensity;
    }

    IEnumerator LerpToDark()
    {
        while (directionnalLight.intensity > darkIntensity)
        {
            directionnalLight.intensity = directionnalLight.intensity - rate;
            //Debug.Log(directionnalLight.intensity);
            yield return 0;
        }

        directionnalLight.intensity = darkIntensity;

        //float alpha = 0;
        //float startTime = Time.time;

        //while (Time.time < startTime + speedFade)
        //{
        //    directionnalLight.intensity = Mathf.Lerp(darkIntensity, standardIntensity, alpha);
        //    yield return 0;
        //}
    }
}
