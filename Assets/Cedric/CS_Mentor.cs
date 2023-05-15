using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Mentor : MonoBehaviour
{
    List<CS_Enemy> enemies = new();


    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {

        }
    }
}
