using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_LightManager : MonoBehaviour
{
    [SerializeField] float speedFade = 2;

    [BoxGroup("DirectionnalLight")] [SerializeField] Light directionnalLight;
    [BoxGroup("DirectionnalLight")] [SerializeField] float rateDirectionnal = 2000;

    float directionnalStandardIntensity;

    [BoxGroup("FireFlies")] [SerializeField] Light fireFliesLight;
    [BoxGroup("FireFlies")] [SerializeField] float rateFireFlies;
    float fireFliesIntensity;

    [BoxGroup("PlayerLight")] [SerializeField] Light playerLight;
    [BoxGroup("PlayerLight")] [SerializeField] float ratePlayer;
    float playerIntensity;

    Coroutine currentLerpCoroutine;

    private void Start()
    {
        CS_TriggerMerger.LightManager = this;
        fireFliesIntensity = fireFliesLight.intensity;
        playerIntensity = playerLight.intensity;
        directionnalStandardIntensity = directionnalLight.intensity;
    }

    public void ChangeSate(bool playerIn)
    {
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
        while (directionnalLight.intensity < directionnalStandardIntensity || fireFliesLight.intensity > 0 || playerLight.intensity > 0)
        {
            directionnalLight.intensity = Mathf.Clamp(directionnalLight.intensity + rateDirectionnal, 0, directionnalStandardIntensity);
            fireFliesLight.intensity = Mathf.Clamp(fireFliesLight.intensity - rateFireFlies, 0, fireFliesIntensity);
            playerLight.intensity = Mathf.Clamp(playerLight.intensity - ratePlayer, 0, playerIntensity);

            yield return 0;
        }

        directionnalLight.intensity = directionnalStandardIntensity;
        fireFliesLight.intensity = playerLight.intensity = 0;

    }

    IEnumerator LerpToDark()
    {
        while (directionnalLight.intensity > 0 || fireFliesLight.intensity < fireFliesIntensity || playerLight.intensity < playerIntensity )
        {
            directionnalLight.intensity = Mathf.Clamp(directionnalLight.intensity - rateDirectionnal, 0, directionnalStandardIntensity);
            fireFliesLight.intensity = Mathf.Clamp(fireFliesLight.intensity + rateFireFlies, 0, fireFliesIntensity);
            playerLight.intensity = Mathf.Clamp(playerLight.intensity + ratePlayer, 0, playerIntensity);

            yield return 0;
        }

        directionnalLight.intensity = 0;
        fireFliesLight.intensity = fireFliesIntensity;
        playerLight.intensity = playerIntensity;
    }
}
