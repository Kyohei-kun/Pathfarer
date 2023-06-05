using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static CS_AnimationEnum;

public class CS_ShieldBrain : CS_Enemy
{
    [HorizontalLine]
    [SerializeField] bool pasBouger;

    bool lastPlayerIsVisible = false;
    bool canMove = true;
    bool attacking = false;

    bool canRotatePlayer = false;

    bool trackPlayer = false;
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
        if (!trackPlayer && !lastPlayerIsVisible && (perceptron.PlayerIsVisible || touched))
        {
            trackPlayer = true;
            ShareMessage(new List<CS_Enemy>());
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
                if (trackPlayer)
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
        trackPlayer = false;
        touched = false;
        agent.SetDestination(startPosition);
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
                canMove = !pasBouger;
                attacking = false;
                canRotatePlayer = true;
                break;
            default:
                break;
        }
    }
}
