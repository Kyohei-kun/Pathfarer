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

    bool aggro;
    bool forceAggro;
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
        if ((!aggro && perceptron.PlayerIsVisible) || forceAggro) Aggro(); 
        else if (aggro && Vector3.Distance(playerTransform.position, myTower.transform.position) > distUnAggro) UnAggro();

        if (!aggro)
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
        aggro = true;
        animCanon.SetFloat("_Offset", animOffset);
        animCanon.SetBool("_Aggro", aggro);

        ShareMessage(new List<CS_Enemy>());

        forceAggro = false;
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

    override public void ShareMessage(List<CS_Enemy> ennemiesMessaged)
    {
        forceAggro = true;
        List<Collider> colliders = Physics.OverlapSphere(transform.position, radiusMessageZone, enemyLayerMask).ToList();

        foreach (Collider collider in colliders)
        {
            CS_Enemy tempEnemy;
            Debug.Log($"Parent : {collider.transform.parent.gameObject.name}");
            Debug.Log($"Moi : {collider.gameObject.name}");

            if (collider.transform.parent.GetComponent<CS_Enemy>() != null)
            {
                tempEnemy = collider.transform.parent.GetComponent<CS_Enemy>(); Debug.Log("Script sur parent !");
            }
            else if (collider.GetComponent<CS_Enemy>() != null)
            {
                tempEnemy = collider.GetComponent<CS_Enemy>(); Debug.Log("Script sur moi");
            }
            else tempEnemy = null; Debug.Log("Script nulle part ! ");

            if (tempEnemy != null && !ennemiesMessaged.Contains(tempEnemy))
            {
                ennemiesMessaged.Add(tempEnemy);
                tempEnemy.ShareMessage(ennemiesMessaged);
            }
        }
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
