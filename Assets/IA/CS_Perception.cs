using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Perception : MonoBehaviour
{
    protected bool playerIsVisible;

    protected Transform playerTransform;

    public void Initialisation(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    public bool PlayerIsVisible { get => playerIsVisible; set => playerIsVisible = value; }
}
