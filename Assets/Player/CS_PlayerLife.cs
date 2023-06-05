using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

public class CS_PlayerLife : MonoBehaviour
{
    [SerializeField] int lifeMax = 3;
    [SerializeField] bool invulnerable = false;
    [Space][ProgressBar("Life", "lifeMax", EColor.Red)][SerializeField] int currentLife;

    [BoxGroup("Feedback PV")][SerializeField] CS_F_Mentor mentor;
    [BoxGroup("Feedback Damage")][SerializeField] AnimationCurve timeScaleCurve;
    [BoxGroup("Feedback Damage")][SerializeField] GameObject fx_Damage;
    [BoxGroup("Feedback Damage Slpash")][SerializeField] Material mat_SplashDamage;
    [BoxGroup("Feedback Damage Slpash")][SerializeField] string animSquishPlane;
    [BoxGroup("Feedback Damage Slpash")][SerializeField] Animator animatorSplash;

    [BoxGroup("Feedback Damage Vignette")][SerializeField] VolumeProfile standard_HDRP_Profile;
    [BoxGroup("Feedback Damage Vignette")][SerializeField] VolumeProfile dark_HDRP_Profile;
    Vignette standard_Vignette;
    Vignette dark_Vignette;
    LensDistortion standard_LensDistortion;
    LensDistortion dark_LensDistortion;

    float durationEffect;
    float timeStartSlowDown = -10;
    Color splashColor;

    CS_PassifEpines epines;

    public int CurrentLife { get => currentLife; set => currentLife = value; }

    void Start()
    {
        currentLife = lifeMax;
        durationEffect = timeScaleCurve.keys[timeScaleCurve.length - 1].time;
        splashColor = mat_SplashDamage.GetColor("_MainColor");
        splashColor.a = 0;
        mat_SplashDamage.SetColor("_MainColor", splashColor);
        standard_HDRP_Profile.TryGet<Vignette>(out standard_Vignette);
        dark_HDRP_Profile.TryGet<Vignette>(out dark_Vignette);
        standard_HDRP_Profile.TryGet<LensDistortion>(out standard_LensDistortion);
        dark_HDRP_Profile.TryGet<LensDistortion>(out dark_LensDistortion);
        epines = GetComponent<CS_PassifEpines>();
    }

    [Button]
    public void LoseLife()
    {
        if (!invulnerable)
        {
            CS_VibrationControler.SetVibration(10, 1, 1f);
            currentLife--;
            currentLife = Mathf.Clamp(currentLife, 0, lifeMax);
            UpdateFXs();
            timeStartSlowDown = Time.unscaledTime;
            GameObject temp = Instantiate(fx_Damage);
            temp.transform.position = transform.position + Vector3.up;
            standard_Vignette.intensity.Override(0.3f);
            dark_Vignette.intensity.Override(0.3f);
            standard_LensDistortion.intensity.Override(0.3f);
            dark_LensDistortion.intensity.Override(0.3f);
            ResetProfils();
            animatorSplash.CrossFade(animSquishPlane, -1, 0);
            Camera.main.GetComponent<CS_CameraUtilities>().Shake(4, 1, 0.8f, true, false);
            //animatorSplash.Play(animSquishPlane);

            epines.Dmg();

        }
    }

    private async void ResetProfils()
    {
        await System.Threading.Tasks.Task.Delay((int)(2 * 1000));
        standard_Vignette.intensity.Override(0);
        dark_Vignette.intensity.Override(0);
        standard_LensDistortion.intensity.Override(0);
        dark_LensDistortion.intensity.Override(0);
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

    public bool FullLife()
    {
        return currentLife == lifeMax;
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