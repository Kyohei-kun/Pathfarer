using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CS_F_HeavyAttack;

public class CS_PlayerSword : MonoBehaviour
{
    [MinValue(0)][SerializeField] int damage = 1;
    [MinValue(0)][SerializeField] int HeavyDamage = 3;
    float bonusDmg = 0;
    bool berserker;
    [MinValue(0)][SerializeField] float berserkerValue = 1;
    [SerializeField] bool isHeavy = false;
    GameObject player;

    public int Damage { get => damage; set => damage = value; }
    public float BonusDmg { get => bonusDmg; set => bonusDmg = value; }
    public bool Berserker { get => berserker; set => berserker = value; }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && berserker && player.GetComponent<CS_PlayerLife>().CurrentLife == 1)
        {
            CS_Enemy ennemi = other.transform.parent.GetComponent<CS_Enemy>();
            if (ennemi != null)
            {
                CS_VibrationControler.SetVibration(1, 1, 0.2f);
                ennemi.TakeDamage(isHeavy? (HeavyDamage + bonusDmg): (damage + bonusDmg), isHeavy? PlayerAttackType.Heavy: PlayerAttackType.Simple);
            }
        }
        else if (other.transform.parent != null)
        {
            CS_Enemy ennemi = other.transform.parent.GetComponent<CS_Enemy>();
            if (ennemi != null)
            {
                CS_VibrationControler.SetVibration(1, 1, 0.2f);
                ennemi.TakeDamage(isHeavy ? (HeavyDamage + bonusDmg + berserkerValue) : (damage + bonusDmg + berserkerValue), isHeavy ? PlayerAttackType.Heavy : PlayerAttackType.Simple);
            }
        }
    }
}