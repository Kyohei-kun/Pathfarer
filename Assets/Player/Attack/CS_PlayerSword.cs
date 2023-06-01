using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CS_F_HeavyAttack;

public class CS_PlayerSword : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] int HeavyDamage = 3;
    [SerializeField] bool isHeavy = false;

    public int Damage { get => damage; set => damage = value; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null)
        {
            CS_Enemy ennemi = other.transform.parent.GetComponent<CS_Enemy>();
            if (ennemi != null)
            {
                CS_VibrationControler.SetVibration(1, 1, 0.2f);
                ennemi.TakeDamage(isHeavy? HeavyDamage: damage, isHeavy? PlayerAttackType.Heavy: PlayerAttackType.Simple);
            }
        }
    }
}