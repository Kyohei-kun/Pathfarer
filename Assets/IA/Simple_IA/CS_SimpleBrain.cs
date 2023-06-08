using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static CS_AnimationEnum;

public class CS_SimpleBrain : CS_Enemy
{
    //Gizmo parameter
    float timerShowGizmoMessage = 5;
    float currenTimerShowGizmoMessage;

    float timerShowGizmoUnagro = 5;
    float currenTimerShowGizmoUnagro;

    NavMeshAgent agent;
    
    bool lastPlayerIsVisible = false;
    bool canMove = true;
    bool attacking = false;
    
    bool canRotatePlayer = false;

    [SerializeField] float speed;
    [SerializeField] float angularSpeed;
    [OnValueChanged("OnRadiusUnagroChange")][SerializeField] float UnagroDistance = 15;

    [OnValueChanged("OnRadiusMessageChange")][SerializeField] float radiusMessageZone = 5;

    [SerializeField] LayerMask enemyLayerMask;
    

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>(); if (agent == null) Debug.LogError("Pas de navmeshAgent");
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
    }
    protected override void OnStartStunning()
    {
        base.OnStartStunning();
        agent.isStopped = false;
        GetComponent<NavMeshAgent>().enabled = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    protected override void OnStopStunning()
    {
        base.OnStopStunning();
        GetComponent<NavMeshAgent>().enabled = true;
        _rigidbody.constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }


    private void Update()
    {
        if (!IsAggro && !lastPlayerIsVisible && (perceptron.PlayerIsVisible || touched || ForceAggro))
        {
            IsAggro = true;
            ForceAggro = false;
        }

        StunUpdate();

        CellMove();
        CellAttack();

        lastPlayerIsVisible = perceptron.PlayerIsVisible;
    }

  


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
            Gizmos.DrawWireSphere(transform.position, UnagroDistance);
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

    private void CellAttack()
    {
        if (!isStun)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) < 1.5f)
            {
                canMove = false;
                attacking = true;
                animator.SetBool("_Attack", true);
            }
        }
    }

    private void CellMove()
    {
        if (!isStun)
        {
            if (canMove)
            {
                if (IsAggro)
                {
                    agent.SetDestination(playerTransform.position);
                    if (agent.path.Lenght() > UnagroDistance) //Si le joueur est trop loin
                    {
                        StopHuntPlayer();
                    }
                }
            }
            else
            {
                if (attacking && canRotatePlayer)
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(playerTransform.position - transform.position, Vector3.up));
                }
            }
        }
    }

    private void StopHuntPlayer()
    {
        IsAggro = false;
        touched = false;
        agent.SetDestination(startPosition);
    }

    public void AnimationEvent(StateSimpleAttack state)
    {
        switch (state)
        {
            case StateSimpleAttack.StopRotate:
                animator.SetBool("_Attack", false);
                canRotatePlayer = false;
                break;
            case StateSimpleAttack.Finish:
                canMove = true;
                attacking = false;
                canRotatePlayer = true;
                break;
            default:
                break;
        }
    }
}
