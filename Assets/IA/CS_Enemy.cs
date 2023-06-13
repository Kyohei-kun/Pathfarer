using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using static CS_F_HeavyAttack;

public class CS_Enemy : MonoBehaviour , CS_I_Attackable
{
    // Général
    protected Rigidbody _rigidbody;
    protected Vector3 startPosition;
    protected Transform playerTransform;
    protected Animator animator;

    // Combat
    protected bool touched = false;
    [BoxGroup("Combat")][MinValue(0)][SerializeField] protected float PV = 3;
    [BoxGroup("Combat")][SerializeField] protected GameObject prefab_DeathFX;
    [BoxGroup("Combat")][SerializeField] bool pushable = true;

    // Stun
    protected bool isStun = false;
    protected bool lastIsStun = false;
    protected float timeStopStunning = -1;
    [BoxGroup("Stun")][SerializeField] bool stunnable = true;
    [BoxGroup("Stun")][SerializeField] Renderer currentRenderer;
    [BoxGroup("Stun")][ShowIf("stunnable")][SerializeField] Material stunningMaterial;
    Material normalMaterial;

    // Aggro
    bool isAggro;
    protected CS_Perception perceptron;
    [BoxGroup("Aggro")][OnValueChanged("OnRadiusUnagroChange")][MinValue(0)][SerializeField] protected float distUnAggro = 6;
    [BoxGroup("Aggro")][OnValueChanged("OnRadiusMessageChange")][MinValue(0)][SerializeField] protected float radiusMessageZone = 5;
    [BoxGroup("Aggro")][SerializeField] LayerMask enemyLayerMask;
    bool lastPlayerIsVisible;

    // Mentor
    protected float baseSpeed = 0;
    [BoxGroup("Mentor")][MinValue(0)][SerializeField] float freezeTime = 2;

    #region Gizmo Parameters
    bool showPermanentGizmo;

    float timerShowGizmoMessage = 5;
    float currenTimerShowGizmoMessage;

    float timerShowGizmoUnagro = 5;
    float currenTimerShowGizmoUnagro;

    [Button]
    private void ShowGizmos() { showPermanentGizmo = !showPermanentGizmo; }
    #endregion

    #region Accesseurs
    protected bool IsAggro { get => isAggro; set => isAggro = value; }
    protected bool LastPlayerIsVisible { get => lastPlayerIsVisible; set => lastPlayerIsVisible = value; }
    #endregion

    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        startPosition = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        normalMaterial = currentRenderer.sharedMaterial;

        perceptron = GetComponentInChildren<CS_Perception>();
        perceptron.Initialisation(playerTransform);
    }

    protected virtual void Update()
    {
        // Aggro
        if (Vector3.Distance(playerTransform.position, transform.position) <= distUnAggro
            && ((!IsAggro && perceptron.PlayerIsVisible) || touched)) Aggro();
        else if (IsAggro && Vector3.Distance(playerTransform.position, transform.position) > distUnAggro) UnAggro();
        
        // Stun
        StunUpdate();
    }

    private void LateUpdate()
    {
        LastPlayerIsVisible = perceptron.PlayerIsVisible;
    }

    #region Stun
    virtual public void Stun(float duration)
    {
        if (!stunnable) return;

        timeStopStunning = Time.time + duration;
    }

    protected virtual void StunUpdate()
    {
        if (!stunnable) return;

        if (Time.time > timeStopStunning)
        {
            isStun = false;
        }
        else
        {
            isStun = true;
        }

        if (lastIsStun != isStun)
        {
            if (isStun) OnStartStunning();
            else OnStopStunning();
        }

        lastIsStun = isStun;
    }

    protected virtual void OnStartStunning()
    {
        if (!stunnable) return;

        animator.SetBool("Stun", true);
        currentRenderer.sharedMaterial = stunningMaterial;
    }

    protected virtual void OnStopStunning()
    {
        if (!stunnable) return;

        animator.SetBool("Stun", false);
        currentRenderer.sharedMaterial = normalMaterial;
    }
    #endregion

    #region Combat
    virtual public void Push(Vector3 force)
    {
        _rigidbody.velocity = force;
    }

    virtual public void TakeDamage(float damage, PlayerAttackType type)
    {
        touched = true;
        PV -= damage;
        Stun(0.5f);

        if (pushable) 
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

        if (PV <= 0)
            Death();
    }

    virtual protected void Death()
    {
        GameObject temp = Instantiate(prefab_DeathFX);
        temp.transform.position = gameObject.transform.position;
        playerTransform.GetComponent<CS_F_Targeting>().RemoveFromTargetableList(gameObject, true);
        Destroy(gameObject);
    }

    #endregion

    #region Aggro
    virtual protected void Aggro()
    {
        IsAggro = true;
    }

    virtual protected void UnAggro()
    {
        IsAggro = false;
        touched = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAggro) return;

        if (other.TryGetComponent(out CS_Enemy scriptEnemy) && scriptEnemy.IsAggro)
        {
            Aggro();
        }
        else if (other.transform.parent && other.transform.parent.TryGetComponent(out scriptEnemy) && scriptEnemy.IsAggro)
        {
            Aggro();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isAggro) return;

        if (other.TryGetComponent(out CS_Enemy scriptEnemy) && scriptEnemy.IsAggro)
        {
            Aggro();
        }
        else if (other.transform.parent && other.transform.parent.TryGetComponent(out scriptEnemy) && scriptEnemy.IsAggro)
        {
            Aggro();
        }
    }
    #endregion

    #region Mentor
    virtual public void FreezeMentor()
    {
        StartCoroutine(WaitUnfreeze());
    }

    IEnumerator WaitUnfreeze()
    {
        yield return new WaitForSeconds(freezeTime);

        UnfreezeMentor();
    }

    protected virtual void UnfreezeMentor() {}
    #endregion

    #region Gizmos
    protected void OnDrawGizmos()
    {
        if (currenTimerShowGizmoMessage < timerShowGizmoMessage || showPermanentGizmo)
        {
            currenTimerShowGizmoMessage += Time.deltaTime;
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, radiusMessageZone);
        }

        if (currenTimerShowGizmoUnagro < timerShowGizmoUnagro || showPermanentGizmo)
        {
            currenTimerShowGizmoUnagro += Time.deltaTime;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, distUnAggro);
        }
    }

    protected void OnRadiusMessageChange()
    {
        currenTimerShowGizmoMessage = 0;
    }

    protected void OnRadiusUnagroChange()
    {
        currenTimerShowGizmoUnagro = 0;
    }
    #endregion
}
