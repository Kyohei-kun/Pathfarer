using NaughtyAttributes;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CS_FeatureUnlocker : MonoBehaviour
{
    [BoxGroup("Scripts Features")][SerializeField] CS_F_Mentor f_Mentor;
    [BoxGroup("Scripts Features")][SerializeField] CS_F_NewAttack f_Attack;
    [BoxGroup("Scripts Features")][SerializeField] CS_F_HeavyAttack f_HeavyAttack;
    [BoxGroup("Scripts Features")][SerializeField] CS_F_FireFlies f_FireFlies;
    [BoxGroup("Scripts Features")][SerializeField] CS_F_Nail f_Nail;
    [BoxGroup("Scripts Features")][SerializeField] ThirdPersonController f_Jump;
    [BoxGroup("Scripts Features")][SerializeField] CS_F_Teleportation f_Teleportation;

    [BoxGroup("Unlock")][SerializeField] bool state_Mentor;
    [BoxGroup("Unlock")][SerializeField] bool state_Attack;
    [BoxGroup("Unlock")][SerializeField] bool state_HeavyAttack;
    [BoxGroup("Unlock")][SerializeField] bool state_FireFlies;
    [BoxGroup("Unlock")][SerializeField] bool state_Nail_Nb1;
    [BoxGroup("Unlock")][SerializeField] bool state_Nail_Nb2;
    [BoxGroup("Unlock")][SerializeField] bool state_Nail_Nb3;
    [BoxGroup("Unlock")][SerializeField] bool state_Nail_Nb4;
    [BoxGroup("Unlock")][SerializeField] bool state_Nail_V2;
    [BoxGroup("Unlock")][SerializeField] bool state_Jump;
    [BoxGroup("Unlock")][SerializeField] bool state_Teleportation_V1;
    [BoxGroup("Unlock")][SerializeField] bool state_Teleportation_Lvl2;

    [BoxGroup("Inventaire")][ShowNonSerializedField] List<string> items = new();

    public bool State_Mentor { get => state_Mentor; set => state_Mentor = value; }
    public bool State_Attack { get => state_Attack; set => state_Attack = value; }
    public bool State_HeavyAttack { get => state_HeavyAttack; set => state_HeavyAttack = value; }
    public bool State_FireFlies { get => state_FireFlies; set => state_FireFlies = value; }
    public bool State_Nail_Lvl1 { get => state_Nail_Nb1; set => state_Nail_Nb1 = value; }
    public bool State_Nail_Lvl2 { get => state_Nail_Nb2; set => state_Nail_Nb2 = value; }
    public bool State_Nail_Lvl3 { get => state_Nail_Nb3; set => state_Nail_Nb3 = value; }
    public bool State_Nail_Lvl4 { get => state_Nail_Nb4; set => state_Nail_Nb4 = value; }
    public bool State_Nail_V2 { get => state_Nail_V2; set => state_Nail_V2 = value; }
    public bool State_Jump { get => state_Jump; set => state_Jump = value; }
    public bool State_Teleportation_Lvl1 { get => state_Teleportation_V1; set => state_Teleportation_V1 = value; }
    public bool State_Teleportation_Lvl2 { get => state_Teleportation_Lvl2; set => state_Teleportation_Lvl2 = value; }

    private void Start()
    {
        ManualUpdate();
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
        if (state_Nail_Nb4)
        {
            f_Nail.NailLevel = 4;
            return;
        }
        else if (state_Nail_Nb3)
        {
            f_Nail.NailLevel = 3;
            return;
        }
        else if (state_Nail_Nb2)
        {
            f_Nail.NailLevel = 2;
            return;
        }
        else if (state_Nail_Nb1)
        {
            f_Nail.NailLevel = 1;
            return;
        }
        else
        {
            f_Nail.NailLevel = 0;
        }
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

        f_Teleportation.TpLevel = state_Teleportation_V1 ? 1:0;
    }

    #region Inventaire
    public void AddInInventory(string itemName)
    {
        items.Add(itemName);
    }

    public bool CheckIfInInventory(string itemName)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Contains(itemName))
            {
                return true;
            }
        }

        return false;
    }
    #endregion
}