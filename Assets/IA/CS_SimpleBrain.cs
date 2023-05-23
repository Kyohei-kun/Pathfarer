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
    Transform playerTransform;
    CS_Perception perceptron;
    Vector3 startPosition;
    bool lastPlayerIsVisible = false;
    bool canMove = true;
    bool attacking = false;
    Animator animator;
    bool canRotatePlayer = false;

    [SerializeField] float speed;
    [SerializeField] float angularSpeed;
    [OnValueChanged("OnRadiusUnagroChange")][SerializeField] float UnagroDistance = 15;

    [OnValueChanged("OnRadiusMessageChange")][SerializeField] float radiusMessageZone = 5;

    bool trackPlayer = false;
    [SerializeField] LayerMask layerMask;

    private void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        agent = GetComponent<NavMeshAgent>(); if (agent == null) Debug.LogError("Pas de navmeshAgent");
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
        perceptron = GetComponent<CS_Perception>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        perceptron.Initialisation(playerTransform);
    }


    private void Update()
    {
        if (!trackPlayer && !lastPlayerIsVisible && perceptron.PlayerIsVisible)
        {
            trackPlayer = true;
            ShareMessage(new List<CS_Enemy>());
        }

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
        if (Vector3.Distance(transform.position, playerTransform.position) < 1.5f)
        {
            canMove = false;
            attacking = true;
            animator.SetBool("_Attack", true);
        }
    }

    private void CellMove()
    {
        if (canMove)
        {
            if (trackPlayer)
            {
                agent.SetDestination(playerTransform.position);
                //Debug.Log(agent.remainingDistance);
                if (agent.path.Lenght() > UnagroDistance)
                {
                    trackPlayer = false;
                    agent.SetDestination(startPosition);
                }
            }
        }
        else
        {
            if(attacking && canRotatePlayer)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(playerTransform.position-transform.position, Vector3.up));
            }
        }
    }

    override public void ShareMessage(List<CS_Enemy> ennemiesMessaged)
    {
        trackPlayer = true;
        List<Collider> colliders = Physics.OverlapSphere(transform.position, radiusMessageZone, layerMask).ToList();

        foreach (Collider collider in colliders)
        {
            if (collider.transform.parent != null)
            {
                CS_Enemy tempEnemy = collider.transform.parent.GetComponent<CS_Enemy>();
                if (tempEnemy != null && !ennemiesMessaged.Contains(tempEnemy))
                {
                    ennemiesMessaged.Add(tempEnemy);
                    tempEnemy.ShareMessage(ennemiesMessaged);
                }
            }
        }
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
