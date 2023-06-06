using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CS_Tower_PV : MonoBehaviour
{
    [Header("Mort")]
    [Required][SerializeField] GameObject fxDeath;

    [Header("Tirs")]
    [MinMaxSlider(0, 1)][SerializeField] float animOffset;
    [Required][SerializeField] GameObject projo;

    GameObject player;
    CS_TowerIA myTower;

    bool aggro;

    Animator animCanon;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myTower = GetComponentInParent<CS_TowerIA>();
        animCanon = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (!aggro) Aggro(); else UnAggro();
        }
    }

    void Aggro()
    {
        aggro = true;
        animCanon.SetFloat("_Offset", animOffset);
        animCanon.SetBool("_Aggro", aggro);

        // Rotation canons vers joueur
    }

    void UnAggro()
    {
        aggro = false;
        animCanon.SetBool("_Aggro", aggro);
    }

    void Attack()
    {
        Instantiate(projo, transform.position + transform.forward, Quaternion.Euler(gameObject.transform.eulerAngles));
    }
}
