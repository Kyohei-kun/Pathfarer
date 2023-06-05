using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CS_F_HeavyAttack;

public class CS_PlayerSword : MonoBehaviour
{
    [MinValue(0)][SerializeField] int simpleDamage = 1;
    [MinValue(0)][SerializeField] int HeavyDamage = 3;
    float bonusDmg = 0;
    bool berserker;
    [MinValue(0)][SerializeField] float berserkerValue = 1;
    [SerializeField] bool isHeavy = false;
    CS_PlayerLife playerlife;

    public int SimpleDamage { get => simpleDamage; set => simpleDamage = value; }
    public float BonusDmg { get => bonusDmg; set => bonusDmg = value; }
    public bool Berserker { get => berserker; set => berserker = value; }
    public float BerserkerValue { get => berserkerValue; set => berserkerValue = value; }

    private void Start()
    {
        playerlife = GameObject.FindGameObjectWithTag("Player").GetComponent<CS_PlayerLife>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CS_I_Attackable attackableObject;

        if (other.transform.parent != null && other.transform.parent.GetComponent<CS_I_Attackable>() != null) //Ennemy
        {
             attackableObject = other.transform.parent.GetComponent<CS_I_Attackable>();
            GiveDamage(attackableObject);
        }
        else if (other.GetComponent<CS_Enemy>() == null) //Object
        {
            attackableObject = other.GetComponent<CS_I_Attackable>();

            if (attackableObject != null)
            {
                GiveDamage(attackableObject);
            }
        }
    }

    private void GiveDamage(CS_I_Attackable attackable)
    {
        CS_VibrationControler.SetVibration(1, 1, 0.2f);
        float totalDamage = 0;

        if (isHeavy) totalDamage = HeavyDamage; else totalDamage = simpleDamage;
        if (berserker && playerlife.CurrentLife == 1) totalDamage += berserkerValue;
        totalDamage += BonusDmg;


        attackable.TakeDamage(totalDamage, isHeavy ? PlayerAttackType.Heavy : PlayerAttackType.Simple);
    }
}