using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static CS_AnimationEnum;
using static CS_F_HeavyAttack;

public class CS_ShieldBrain : CS_Enemy
{
    [HorizontalLine]
    [SerializeField] bool pasBouger;

    bool lastPlayerIsVisible = false;
    bool canMove = true;
    bool attacking = false;

    bool canRotatePlayer = false;

    [SerializeField] LayerMask layerMask;

    NavMeshAgent agent;
    #region Gizmo parameter
    readonly float timerShowGizmoMessage = 5;
    float currenTimerShowGizmoMessage;

    readonly float timerShowGizmoUnagro = 5;
    float currenTimerShowGizmoUnagro;

    [OnValueChanged("OnRadiusUnagroChange")][SerializeField] float UnagroDistance = 15;
    [OnValueChanged("OnRadiusMessageChange")][SerializeField] float radiusMessageZone = 5;

    private void OnRadiusMessageChange()
    {
        currenTimerShowGizmoMessage = 0;
    }

    private void OnRadiusUnagroChange()
    {
        currenTimerShowGizmoUnagro = 0;
    }
    #endregion

    protected override void Start()
    {
        base.Start();
        if (!TryGetComponent<NavMeshAgent>(out agent)) Debug.LogError($"{gameObject.name} n'a pas de navmeshAgent !");

        if (pasBouger)
        {
            agent.speed = 0;
            canMove = !pasBouger;
        }
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

    private void CellAttack()
    {
        if (!isStun)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) < 1.5f)
            {
                Vector3 playerDirection = (playerTransform.position - transform.position).normalized; //IA to Player
                playerDirection = Vector3.ProjectOnPlane(playerDirection, Vector3.up);

                if (Vector3.Dot(transform.forward, playerDirection) > 0.5f)
                {
                    canMove = false;
                    attacking = true;
                    animator.SetBool("_Attack", true);
                }
                else
                {

                }
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
                    Vector3 playerDirection = (playerTransform.position - transform.position).normalized; //IA to Player
                    playerDirection = Vector3.ProjectOnPlane(playerDirection, Vector3.up);


                    if (agent.path.Lenght() > UnagroDistance) //Si le joueur est trop loin et face au joueur
                    {
                        StopHuntPlayer();
                    }
                    if ((agent.path.Lenght() > 1.5f || agent.path.Lenght() == -1 ))
                    {
                        agent.SetDestination(playerTransform.position);
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
                canMove = !pasBouger;
                attacking = false;
                canRotatePlayer = true;
                break;
            default:
                break;
        }
    }

    public override void TakeDamage(float damage, PlayerAttackType type)
    {
        Debug.Log("BONJOUR");
        touched = true;

        Vector3 attackDirection = (transform.position - playerTransform.position).normalized;
        attackDirection = Vector3.ProjectOnPlane(attackDirection, Vector3.up);

        if (Vector3.Dot(transform.forward, attackDirection) > 0)
        {
            PV -= damage;
            Stun(0.5f);
            switch (type)
            {
                case PlayerAttackType.Simple:
                    Push(GameObject.FindGameObjectWithTag("Player").transform.forward * 15);
                    break;
                case PlayerAttackType.Heavy:
                    Push(GameObject.FindGameObjectWithTag("Player").transform.forward * 60);
                    break;
                case PlayerAttackType.Pilon:
                    Push((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).normalized * 15);
                    break;
                case PlayerAttackType.Epines:
                    Push((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).normalized * 30);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case PlayerAttackType.Simple:
                    Push(GameObject.FindGameObjectWithTag("Player").transform.forward * 15/2);
                    break;
                case PlayerAttackType.Heavy:
                    Push(GameObject.FindGameObjectWithTag("Player").transform.forward * 60/2);
                    break;
                case PlayerAttackType.Pilon:
                    Push((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).normalized * 15/2);
                    break;
                case PlayerAttackType.Epines:
                    Push((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).normalized * 30/2);
                    break;
                default:
                    break;
            }
        }

        if (PV <= 0)
            Death();
    }
}
