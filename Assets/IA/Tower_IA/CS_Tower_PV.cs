using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

public class CS_Tower_PV : CS_Enemy
{
    [BoxGroup("Tirs")][Range(0,1)][SerializeField] float animOffset = 0;
    [BoxGroup("Tirs")][Required][SerializeField] GameObject projo;

    CS_TowerIA myTower;

    Vector3 newRot;
    [BoxGroup("Aggro")][MinValue(0)][SerializeField] float rotSpeed = 4;
    [BoxGroup("Aggro")][OnValueChanged("OnRadiusUnagroChange")][MinValue(0)][SerializeField] float distUnAggro = 6;
    [BoxGroup("Aggro")][OnValueChanged("OnRadiusMessageChange")][SerializeField] float radiusMessageZone = 5;
    [BoxGroup("Aggro")][SerializeField] LayerMask enemyLayerMask;

    #region Gizmo Parameters
    float timerShowGizmoMessage = 5;
    float currenTimerShowGizmoMessage;

    float timerShowGizmoUnagro = 5;
    float currenTimerShowGizmoUnagro;
    #endregion

    Animator animCanon;

    protected override void Start()
    {
        base.Start();

        myTower = GetComponentInParent<CS_TowerIA>();
        animCanon = GetComponent<Animator>();
        newRot = transform.eulerAngles;
    }

    private void Update()
    {
        if ((!IsAggro && perceptron.PlayerIsVisible) || ForceAggro) Aggro(); 
        else if (IsAggro && Vector3.Distance(playerTransform.position, myTower.transform.position) > distUnAggro) UnAggro();

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

    void Aggro()
    {
        IsAggro = true;
        animCanon.SetFloat("_Offset", animOffset);
        animCanon.SetBool("_Aggro", IsAggro);

        ForceAggro = false;
    }

    void UnAggro()
    {
        IsAggro = false;
        animCanon.SetBool("_Aggro", IsAggro);

        GetComponentInChildren<Transform>().localEulerAngles = Vector3.zero;
    }

    void Attack()
    {
        Instantiate(projo, transform.position + (transform.forward * 1f), Quaternion.Euler(gameObject.transform.eulerAngles));
    }

    protected override void Death()
    {
        base.Death();

        myTower.RemovePV(this);
    }

    #region Gizmos
    private void OnDrawGizmos()
    {
        if (currenTimerShowGizmoMessage < timerShowGizmoMessage)
        {
            currenTimerShowGizmoMessage += Time.deltaTime;
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, radiusMessageZone);
        }

        if (currenTimerShowGizmoUnagro < timerShowGizmoUnagro)
        {
            currenTimerShowGizmoUnagro += Time.deltaTime;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, distUnAggro);
        }
    }

    private void OnRadiusMessageChange()
    {
        currenTimerShowGizmoMessage = 0;
    }

    private void OnRadiusUnagroChange()
    {
        currenTimerShowGizmoUnagro = 0;
    }
    #endregion
}
