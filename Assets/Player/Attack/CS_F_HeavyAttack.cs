using NaughtyAttributes;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CS_F_HeavyAttack : MonoBehaviour
{
    //[HorizontalLine(color: EColor.Red)]
    [BoxGroup("Parameter")] [SerializeField] float knockBackForce = 0;
    [BoxGroup("Parameter")] [SerializeField] float timeCharge = 1f;
    [BoxGroup("Parameter")] [SerializeField] float cooldown = 1f;

    [BoxGroup("Dash")] [SerializeField] float speed_SideDash = 1f;
    [BoxGroup("Dash")] [SerializeField] float speed_DepthDash = 1f;

    [BoxGroup("Visuel")] [SerializeField] GameObject slash_GO;
    [BoxGroup("Visuel")] [SerializeField] GameObject charge_FX;
    [BoxGroup("Visuel")] [SerializeField] float timeShowAttack = 0.2f;
    [BoxGroup("Visuel")] [ProgressBar("CoolDown", "cooldown", EColor.Green)] [SerializeField] private float currentCoolDown = 0;

    ThirdPersonController thirdPersonController;
    CharacterController _controller;

    private bool inCharge = false;
    private float currentCharge = 0;
    private bool inputDown = false;
    private bool lastInputDown = false;
    private bool inCoolDown = false;
    private bool canAttack = true;


    private void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (inCoolDown)
        {
            currentCoolDown += Time.deltaTime;
            if (currentCoolDown >= cooldown)
            {
                inCoolDown = false;
                canAttack = true;
                currentCoolDown = 0;
            }
        }

        if (currentCoolDown >= timeShowAttack)
            slash_GO.SetActive(false);

        if (inCharge)
            currentCharge += Time.deltaTime;

        if (inputDown && !lastInputDown && canAttack)
        {
            thirdPersonController.CanMove = false;
            charge_FX.SetActive(true);
            currentCharge = 0;
            inCharge = true;
            canAttack = false;
        }

        if (inCharge && currentCharge >= timeCharge)
        {
            Dash();
            slash_GO.SetActive(true);
            charge_FX.SetActive(false);
            inCharge = false;
            thirdPersonController.CanMove = true;
            inCoolDown = true;
        }
    }

    public void OnBigAttack(CallbackContext context)
    {
        inputDown = context.ReadValueAsButton();
    }

    private void Dash()
    {
        float finalSpeed = Mathf.Lerp(speed_SideDash, speed_DepthDash, Mathf.Abs(Vector3.Dot(thirdPersonController.transform.forward, new Vector3(-1, 0, 1).normalized)));
        _controller.SimpleMove(_controller.transform.forward * finalSpeed * Time.deltaTime);
    }
}
