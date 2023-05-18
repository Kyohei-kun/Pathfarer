using NaughtyAttributes;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_FeatureUnlocker : MonoBehaviour
{
    [BoxGroup("Scripts Features")][SerializeField] CS_F_Mentor f_Mentor;
    [BoxGroup("Scripts Features")][SerializeField] CS_F_Attack f_Attack;
    [BoxGroup("Scripts Features")][SerializeField] CS_F_HeavyAttack f_HeavyAttack;
    [BoxGroup("Scripts Features")][SerializeField] CS_F_FireFlies f_FireFlies;
    [BoxGroup("Scripts Features")][SerializeField] CS_F_Nail f_Nail;
    [BoxGroup("Scripts Features")][SerializeField] ThirdPersonController f_Jump;
    [BoxGroup("Scripts Features")][SerializeField] CS_F_Teleportation f_Teleportation;

    [BoxGroup("Unlock")][SerializeField] bool state_Mentor;
    [BoxGroup("Unlock")][SerializeField] bool state_Attack;
    [BoxGroup("Unlock")][SerializeField] bool state_HeavyAttack;
    [BoxGroup("Unlock")][SerializeField] bool state_FireFlies;
    [BoxGroup("Unlock")][SerializeField] bool state_Nail;
    [BoxGroup("Unlock")][SerializeField] bool state_Jump;
    [BoxGroup("Unlock")][SerializeField] bool state_Teleportation_Lvl1;
    [BoxGroup("Unlock")][SerializeField] bool state_Teleportation_Lvl2;

    private void Start()
    {
        //ManualUpdate();
    }

    [Button]
    public void ManualUpdate()
    {
        UpdateMentor();
        UpdateAttack();
        UpdateHeavyAttack();
        UpdateFireflies();
        UpdateNail();
        UpdateJump();
        UpdateTP();
    }


    private void UpdateMentor()
    {
        f_Mentor.gameObject.SetActive(state_Mentor ? true : false);
    }

    private void UpdateAttack()
    {
        f_Attack.enabled = (state_Attack ? true : false);
    }

    private void UpdateHeavyAttack()
    {
        f_HeavyAttack.enabled = (state_HeavyAttack ? true : false);
    }

    private void UpdateFireflies()
    {
        if (state_FireFlies)
            f_FireFlies.UnlockFeature();
        else
            f_FireFlies.LockFeature();
    }

    private void UpdateNail()
    {
        f_Nail.FeatureIsUnlocked = (state_Nail ? true : false);
    }

    private void UpdateJump()
    {
        f_Jump.FeatureJumpUnlocked = (state_Jump ? true : false);
    }

    private void UpdateTP()
    {
        if (state_Teleportation_Lvl2)
        {
            f_Teleportation.TpLevel = 2;
            return;
        }

        f_Teleportation.TpLevel = state_Teleportation_Lvl1 ? 1:0;
    }
}