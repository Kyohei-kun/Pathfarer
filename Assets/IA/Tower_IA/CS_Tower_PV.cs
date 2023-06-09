using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class CS_Tower_PV : CS_Enemy
{
    Animator animCanon;

    [BoxGroup("Tirs")][Range(0,1)][SerializeField] float animOffset = 0;
    [BoxGroup("Tirs")][Required][SerializeField] GameObject projo;

    CS_TowerIA myTower;

    Vector3 newRot;
    [MinValue(0)][SerializeField] float rotSpeed = 4;

    protected override void Start()
    {
        base.Start();

        myTower = GetComponentInParent<CS_TowerIA>();
        animCanon = GetComponent<Animator>();
        newRot = transform.eulerAngles;
    }

    protected override void Update()
    {
        base.Update();

        // Aggro
        if (IsAggro && Vector3.Distance(playerTransform.position, myTower.transform.position) > distUnAggro) UnAggro();

        // Animations
        if (!IsAggro)
        {
            newRot.y += Time.deltaTime * rotSpeed;
            transform.eulerAngles = newRot;
        }
        else
        {
            transform.LookAt(new Vector3 (playerTransform.position.x, transform.position.y, playerTransform.position.z));
            transform.GetChild(0).GetComponent<Transform>().LookAt(playerTransform.position + Vector3.up);
        }
    }

    #region Aggro
    protected override void Aggro()
    {
        base.Aggro();

        animCanon.SetFloat("_Offset", animOffset);
        animCanon.SetBool("_Aggro", IsAggro);
    }

    protected override void UnAggro()
    {
        base.UnAggro();

        animCanon.SetBool("_Aggro", IsAggro);

        GetComponentInChildren<Transform>().localEulerAngles = Vector3.zero;
    }
    #endregion

    #region Combat
    void Attack()
    {
        Instantiate(projo, transform.position + (GetComponent<Transform>().forward * 0.9f), Quaternion.identity);
    }

    protected override void Death()
    {
        base.Death();

        myTower.RemovePV(this);
    }
    #endregion
}
