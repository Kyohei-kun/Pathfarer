using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TriggerLight : MonoBehaviour
{
    bool playerIN = false;

    public bool PlayerIN { get => playerIN;}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerIN = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIN = false;
        }
    }
}
