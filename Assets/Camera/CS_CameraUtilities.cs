using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CameraUtilities : MonoBehaviour
{
    private CinemachineBrain cinemachineBrain;


    private void Start()
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();
    }

    public void Shake(float amplitude, float frequency, float duration)
    {
        CinemachineBasicMultiChannelPerlin noise = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StartCoroutine(Co_Shake(noise, amplitude, frequency, duration));
    }

    private IEnumerator Co_Shake(CinemachineBasicMultiChannelPerlin noise, float amplitude, float frequency, float duration)
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;

        yield return new WaitForSeconds(duration);

        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }

    private void OnDestroy()
    {
        CinemachineBasicMultiChannelPerlin noise;

        try
        {
            noise = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        catch (System.Exception)
        {
            return;
        }
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
}
