using NaughtyAttributes;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SimpleAttackSender : MonoBehaviour
{
    [MinValue(0)][SerializeField] float knockbackForce = 100;
    [Required][SerializeField] Transform enemy;
    [SerializeField] bool makeDamage = false;

    private void OnTriggerEnter(Collider other)
    {
        if (makeDamage)
        {
            CS_PlayerLife playerLife = other.GetComponent<CS_PlayerLife>();
            if (playerLife != null)
            {
                playerLife.LoseLife();
                Vector3 pushVector = enemy.forward;
                other.GetComponent<ThirdPersonController>().Push(pushVector, knockbackForce);
            }
        }
    }
}
