using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SimpleAttackSender : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CS_PlayerLife playerLife = other.GetComponent<CS_PlayerLife>();
        if(playerLife != null)
        {
            playerLife.LoseLife();
            Vector3 pushVector = transform.parent.parent.forward;
            other.GetComponent<ThirdPersonController>().Push(pushVector, 100f);
        }
    }
}
