using NaughtyAttributes;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

enum ComboState
{
    Neutral = 0,
    First = 1,
    Second = 2,
    Final = 3
}

public class CS_F_Attack : MonoBehaviour
{
    [BoxGroup("Visuel")][SerializeField][HorizontalLine(color: EColor.Gray)] GameObject slashFirst;
    [BoxGroup("Visuel")][SerializeField] GameObject slashSecond;
    [BoxGroup("Visuel")][SerializeField] GameObject slashFinal;

    [BoxGroup("Speed_Dash")][SerializeField][HorizontalLine(color: EColor.Gray)] float speed_DepthFirstDash;
    [BoxGroup("Speed_Dash")][SerializeField] float speed_SideFirstDash;
    [BoxGroup("Speed_Dash")][SerializeField] float speed_DepthFinalDash;
    [BoxGroup("Speed_Dash")][SerializeField] float speed_SideFinalDash;

    [BoxGroup("CoolDown")][SerializeField][HorizontalLine(color: EColor.Gray)] float coolDownFirst = 0.5f;
    [BoxGroup("CoolDown")][SerializeField] float coolDownSecond = 0.5f;
    [BoxGroup("CoolDown")][SerializeField] float coolDownFinal = 1f;

    [BoxGroup("Divers")][SerializeField][HorizontalLine(color: EColor.Gray)] float timeShowAttack = 1f;
    [BoxGroup("Divers")][SerializeField] float timeInterCombo = 1f;

    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

    [Foldout("■■ AirAttack ■■")][SerializeField][HorizontalLine(color: EColor.Red)] float timeMomentum = 0.5f;
    [Foldout("■■ AirAttack ■■")][SerializeField] float coolDownAirAttack = 0.2f;
    [Foldout("■■ AirAttack ■■")][SerializeField] float timeShowAirAttack = 0.2f;
    [Foldout("■■ AirAttack ■■")][SerializeField] float speedMomentum = 0.2f;
    [Foldout("■■ AirAttack ■■")][SerializeField] int nbAirAttack = 3;

    private bool canAirAttack = true;
    private int currentNbAirAttack = 0;
    private float currentAirAttackCoolDown = 0;
    private float currentCoolDown = 0;
    private float cooldownTarget = 0;
    private bool canAttack = true;
    private bool inputDown;
    private bool lastInputDown = false;
    private ComboState combo = ComboState.Neutral;
    private CharacterController _controller;
    private ThirdPersonController thirdPersonController;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        thirdPersonController.MomentumVerticalVelocity = speedMomentum;

    }

    public void OnAttack(CallbackContext context)
    {
        if (this.enabled)
        {
            inputDown = context.ReadValueAsButton();
        }
    }

    private void Update()
    {
        if (inputDown)
        {
            thirdPersonController.InMomentum = true;
        }

        currentCoolDown += Time.deltaTime;
        currentAirAttackCoolDown += Time.deltaTime;

        if (currentCoolDown >= timeShowAttack && thirdPersonController.grounded)
        {
            HideAttack();
            thirdPersonController.CanRotate = true;
        }

        if (currentCoolDown >= cooldownTarget) //Si le precedent cooldown est passé
        {
            canAttack = true;

            if (currentCoolDown >= cooldownTarget + timeInterCombo && combo != ComboState.Neutral)
            {
                combo = ComboState.Neutral;
                cooldownTarget = 0;
            }
            if (combo == ComboState.Final)
            {
                combo = ComboState.Neutral;
                cooldownTarget = 0;
            }
        }

        if (inputDown && !lastInputDown)//Input down
        {
            if (thirdPersonController.grounded)
            {
                if (canAttack)
                {
                    thirdPersonController.CanRotate = false;
                    NextAttackCombo();
                    Dash(combo);
                    DrawAttack(combo);
                    currentCoolDown = 0;
                    canAttack = false;
                }
            }
            else  //Air Attack
            {
                if (currentNbAirAttack < nbAirAttack && canAirAttack && thirdPersonController.VerticalVelocity < 0)
                {
                    thirdPersonController.InMomentum = true;
                    DrawAttack(ComboState.First);
                    canAirAttack = false;
                    currentNbAirAttack++;
                    currentAirAttackCoolDown = 0;
                }
            }
        }

        if (thirdPersonController.grounded)
            currentNbAirAttack = 0;

        if (!thirdPersonController.grounded && currentAirAttackCoolDown >= timeShowAirAttack)
            HideAttack();

        if (currentAirAttackCoolDown >= timeMomentum)
        {
            thirdPersonController.InMomentum = false;
        }

        if (currentAirAttackCoolDown >= coolDownAirAttack)
        {
            canAirAttack = true;
        }

        lastInputDown = inputDown;
    }

    private void Dash(ComboState combo)
    {
        float finalSpeed = Mathf.Lerp(combo == ComboState.Final ? speed_SideFinalDash : speed_SideFirstDash, combo == ComboState.Final ? speed_DepthFinalDash : speed_DepthFirstDash, Mathf.Abs(Vector3.Dot(_controller.transform.forward, new Vector3(-1, 0, 1).normalized)));
        _controller.SimpleMove(_controller.transform.forward * finalSpeed * Time.deltaTime);
    }

    private void HideAttack()
    {
        slashFirst.SetActive(false);
        slashSecond.SetActive(false);
        slashFinal.SetActive(false);
    }

    private void DrawAttack(ComboState combo)
    {
        //Debug.Log("ATTACK  " + combo);
        switch (combo)
        {
            case ComboState.First:
                slashFirst.SetActive(true);
                break;
            case ComboState.Second:
                slashSecond.SetActive(true);
                break;
            case ComboState.Final:
                slashFinal.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void NextAttackCombo()
    {
        combo += 1;
        if ((int)combo == (int)ComboState.Final + 1)
        {
            combo = ComboState.Neutral;
            cooldownTarget = 0;
        }

        switch (combo)
        {
            case ComboState.First:
                cooldownTarget = coolDownFirst;
                break;
            case ComboState.Second:
                cooldownTarget = coolDownSecond;
                break;
            case ComboState.Final:
                cooldownTarget = coolDownFinal;
                break;
            default:
                break;
        }
    }
}