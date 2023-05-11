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

    public void Shake(float amplitude, float frequency, float duration, bool smooth, bool waitOneFrame)
    {
        CinemachineBasicMultiChannelPerlin noise = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StartCoroutine(Co_Shake(noise, amplitude, frequency, duration, smooth, waitOneFrame));
    }

    private IEnumerator Co_Shake(CinemachineBasicMultiChannelPerlin noise, float amplitude, float frequency, float duration, bool smooth, bool waitOneFrame)
    {
        if (waitOneFrame)
            yield return 0;

        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;

        if (smooth)
        {
            float startTime = Time.time;
            while (Time.time < startTime + duration)
            {
                float alpha = Time.time.Remap(startTime, startTime + duration, 0, 1);

                noise.m_AmplitudeGain = Mathf.Lerp(amplitude, 0, alpha);
                noise.m_FrequencyGain = Mathf.Lerp(frequency, 0, alpha);

                yield return 0;
            }
        }
        else
        {
            yield return new WaitForSeconds(duration);

            noise.m_AmplitudeGain = 0;
            noise.m_FrequencyGain = 0;
        }

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
