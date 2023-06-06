using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class CS_Tower_PV : MonoBehaviour
{
    [Header("Mort")]
    [Required][SerializeField] GameObject fxDeath;

    [Header("Tirs")]
    [Range(0,1)][SerializeField] float animOffset = 0;
    [Required][SerializeField] GameObject projo;

    GameObject player;
    CS_TowerIA myTower;

    [Header("Aggro")]
    [MinValue(0)][SerializeField] float rotSpeed = 4;
    [MinValue(0)][SerializeField] float distAggro = 4;
    [MinValue(0)][SerializeField] float fieldVision = 160;
    [ValidateInput("GreaterThanDistAggro", "distUnAggro doit être plus grande que distAggro !")]
    [MinValue(0)][SerializeField] float distUnAggro = 6;
    bool aggro;
    Vector3 newRot;

    Animator animCanon;

    bool GreaterThanDistAggro(float value) { return value > distAggro; }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myTower = GetComponentInParent<CS_TowerIA>();
        animCanon = GetComponent<Animator>();
        newRot = transform.eulerAngles;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (!aggro) Aggro(); else UnAggro();
        }

        if (!aggro)
        {
            newRot.y += Time.deltaTime * rotSpeed;
            transform.eulerAngles = newRot;
        }
        else
        {
            transform.LookAt(new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z));
            transform.GetChild(0).GetComponent<Transform>().LookAt(player.transform.position + Vector3.up);
        }
    }

    void Aggro()
    {
        aggro = true;
        animCanon.SetFloat("_Offset", animOffset);
        animCanon.SetBool("_Aggro", aggro);
    }

    void UnAggro()
    {
        aggro = false;
        animCanon.SetBool("_Aggro", aggro);

        GetComponentInChildren<Transform>().localEulerAngles = Vector3.zero;
    }

    void Attack()
    {
        Instantiate(projo, transform.position + (transform.forward * 1f), Quaternion.Euler(gameObject.transform.eulerAngles));
    }
}
