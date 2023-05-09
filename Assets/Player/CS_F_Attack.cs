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

    [ProgressBar("Combo", 4, EColor.Blue)]
    public int debugCombo;

    ComboState combo = ComboState.Neutral;
    CharacterController _controller;
    ThirdPersonController thirdPersonController;

    bool inputDown;
    bool lastInputDown = false;

    [SerializeField] GameObject slashFirst;
    [SerializeField] GameObject slashSecond;
    [SerializeField] GameObject slashFinal;

    [HorizontalLine(color: EColor.Red)]
    [BoxGroup("Speed_Dash")] [SerializeField] float speed_DepthFirstDash;
    [BoxGroup("Speed_Dash")] [SerializeField] float speed_SideFirstDash;

    [BoxGroup("Speed_Dash")] [SerializeField] float speed_DepthFinalDash;
    [BoxGroup("Speed_Dash")] [SerializeField] float speed_SideFinalDash;

    [HorizontalLine(color: EColor.Red)]
    [BoxGroup("CoolDown")] [SerializeField] float coolDownFirst = 0.5f;
    [BoxGroup("CoolDown")] [SerializeField] float coolDownSecond = 0.5f;
    [BoxGroup("CoolDown")] [SerializeField] float coolDownFinal = 1f;

    [HorizontalLine(color: EColor.Green)]
    [BoxGroup("Divers")] [SerializeField] float timeShowAttack = 1f;
    [BoxGroup("Divers")] [SerializeField] float timeInterCombo = 1f;

    private float currentCoolDown = 0;
    private float cooldownTarget = 0;

    private bool canAttack = true;

    private int i = 0;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        //Application.targetFrameRate = 20;
    }

    public void OnAttack(CallbackContext context)
    {
        inputDown = context.ReadValueAsButton();
    }


    private void Update()
    {
        debugCombo = (int)combo;

        currentCoolDown += Time.deltaTime;

        if (currentCoolDown >= timeShowAttack)
        {
            HideAttack();
            thirdPersonController.canRotate = true;
        }

        if (currentCoolDown >= cooldownTarget) //Si le precedent cooldown est passé
        {
            canAttack = true;

            if (currentCoolDown >= cooldownTarget + timeInterCombo && combo != ComboState.Neutral)
            {
                combo = ComboState.Neutral;
                cooldownTarget = 0;
                //Debug.Log("RESET");
                //Debug.Log("Reset Combo");
            }
            if(combo == ComboState.Final)
            {
                combo = ComboState.Neutral;
                cooldownTarget = 0;
            }
        }

        if (inputDown && !lastInputDown && canAttack)//Input down
        {
            i++;
            thirdPersonController.canRotate = false;
            NextAttackCombo();
            Dash(combo);
            DrawAttack(combo);
            currentCoolDown = 0;
            canAttack = false;
            //Debug.Log("Input " + i + "  " + cooldownTarget);
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