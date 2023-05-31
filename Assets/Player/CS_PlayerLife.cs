using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CS_PlayerLife : MonoBehaviour
{
    [SerializeField] CS_F_Mentor mentor;
    [SerializeField] AnimationCurve timeScaleCurve;
    [SerializeField] GameObject fx_Damage;
    [SerializeField] Material mat_SplashDamage;
    
    float durationEffect;
    float timeStartSlowDown = -10;
    Color splashColor;

    [SerializeField] int lifeMax = 3;
    [Space] [ProgressBar("Life", "lifeMax", EColor.Red)] [SerializeField] int currentLife;

    void Start()
    {
        currentLife = lifeMax;
        durationEffect = timeScaleCurve.keys[timeScaleCurve.length - 1].time;
        splashColor = mat_SplashDamage.GetColor("_MainColor");
        splashColor.a = 0;
        mat_SplashDamage.SetColor("_MainColor", splashColor);
    }

    [Button]
    public void LoseLife()
    {
        CS_VibrationControler.SetVibration(10, 1, 1f);
        currentLife--;
        currentLife = Mathf.Clamp(currentLife, 0, lifeMax);
        UpdateFXs();
        timeStartSlowDown = Time.unscaledTime;
        GameObject temp = Instantiate(fx_Damage);
        temp.transform.position = transform.position + Vector3.up;
    }

    [Button]
    public void GainLife()
    {
        currentLife++;
        currentLife = Mathf.Clamp(currentLife, 0, lifeMax);
        UpdateFXs();
    }

    private void UpdateFXs()
    {
        mentor.UpdateVisuel_Life(currentLife);
    }

    // Update temporaire pour boutton clavier !
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            GoToCheckpoint();
        }
        if (Time.unscaledTime < timeStartSlowDown + durationEffect)
        {
            float alpha = Time.unscaledTime.Remap(timeStartSlowDown, timeStartSlowDown + durationEffect, 0, 1);
            Time.timeScale = timeScaleCurve.Evaluate(alpha);
            splashColor.a = alpha;
            mat_SplashDamage.SetColor("_MainColor", splashColor);
        }
        else
        {
            splashColor.a = 0;
            mat_SplashDamage.SetColor("_MainColor", splashColor);
            Time.timeScale = 1;
        }
    }


    [Button]
    void GoToCheckpoint()
    {
        if (CS_Checkpoints.actualCheckpoint != null)
        {
            GetComponent<CharacterController>().enabled = false;

            transform.position = CS_Checkpoints.actualCheckpoint.transform.position + (Vector3.up * 0.3f);

            GetComponent<CharacterController>().enabled = true;
        }
        else
        {
            Debug.LogError($"Le joueur n'as actuellement pas de checkpoint activé !\n" +
                $"Ca peut être par ce qu'il meurt avant d'en avoir trigger un, ou a cause d'une erreure de code.\n" +
                $"Nombres de checkpoint connus : {CS_Checkpoints.checkpointsConnus.Count}.");
        }
    }
}