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
    NavMeshAgent agent;
    
    bool canMove = true;
    bool attacking = false;
    
    bool canRotatePlayer = false;

    protected override void Start()
    {
        base.Start();

        if (!TryGetComponent(out agent)) Debug.LogError($"{gameObject.name} n'a pas de navmeshAgent !");
    }
    protected override void Update()
    {
        base.Update();

        CellMove();
        CellAttack();
    }

    #region Stun
    protected override void OnStartStunning()
    {
        base.OnStartStunning();

        agent.isStopped = false;
        agent.enabled = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    protected override void OnStopStunning()
    {
        base.OnStopStunning();

        agent.enabled = true;
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.velocity = Vector3.zero;
    }
    #endregion

    #region Combat
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
    #endregion

    #region Aggro
    protected override void UnAggro()
    {
        base.UnAggro();

        agent.SetDestination(startPosition);
    }
    #endregion

    #region Comportement
    private void CellMove()
    {
        if (!isStun)
        {
            if (canMove)
            {
                if (IsAggro)
                {
                    agent.SetDestination(playerTransform.position);

                    if (agent.path.Lenght() > distUnAggro) //Si le joueur est trop loin
                    {
                        UnAggro();
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
    #endregion
}
