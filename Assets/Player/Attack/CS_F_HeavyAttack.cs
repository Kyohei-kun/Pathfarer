using Cinemachine;
using NaughtyAttributes;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CS_F_HeavyAttack : MonoBehaviour
{
    //[HorizontalLine(color: EColor.Red)]
    [BoxGroup("Parameter")][SerializeField] float knockBackForce = 0;
    [BoxGroup("Parameter")][SerializeField] float timeCharge = 1f;
    [BoxGroup("Parameter")][SerializeField] float cooldown = 1f;

    [BoxGroup("Dash")][SerializeField] float speed_SideDash = 1f;
    [BoxGroup("Dash")][SerializeField] float speed_DepthDash = 1f;

    [BoxGroup("Visuel")][SerializeField] GameObject slash_GO;
    [BoxGroup("Visuel")][SerializeField] GameObject charge_FX;
    [BoxGroup("Visuel")][SerializeField] float timeShowAttack = 0.2f;
    [BoxGroup("Visuel")][ProgressBar("CoolDown", "cooldown", EColor.Green)][SerializeField] private float currentCoolDown = 0;

    [Foldout("■■ AirAttack ■■")][SerializeField] GameObject prefab_SpeedPlane;
    [Foldout("■■ AirAttack ■■")][SerializeField] GameObject prefab_FxGround;
    [Foldout("■■ AirAttack ■■")][SerializeField] float minDistanceGround;
    [Foldout("■■ AirAttack ■■")][SerializeField] float timeShake;
    [Foldout("■■ AirAttack ■■")][SerializeField] float amplitude;
    [Foldout("■■ AirAttack ■■")][SerializeField] float frequence;
    [Foldout("■■ AirAttack ■■")][SerializeField] LayerMask layerMask;
    [Foldout("■■ AirAttack ■■")][SerializeField] CinemachineVirtualCamera virtualCam;

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

        if (inputDown && !lastInputDown)
        {
            if (thirdPersonController.grounded)
            {
                if (canAttack)
                {
                    thirdPersonController.CanMove = false;
                    charge_FX.SetActive(true);
                    currentCharge = 0;
                    inCharge = true;
                    canAttack = false;
                }
            }
            else
            {
                if (thirdPersonController.VerticalVelocity < 0) //Attack PILON
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
                    {
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") && Vector3.Distance(hit.point, transform.position) >= minDistanceGround)
                        {
                            //Debug.Break();
                            GameObject speedPlane = GameObject.Instantiate(prefab_SpeedPlane);
                            GameObject fx_Ground = GameObject.Instantiate(prefab_FxGround);
                            fx_Ground.transform.position = hit.point;
                            speedPlane.transform.localScale = new Vector3(1, Vector3.Distance(transform.position, hit.point), 1);
                            speedPlane.transform.position = (transform.position + hit.point) / 2f + Vector3.up * 0.5f;
                            _controller.enabled = false;
                            thirdPersonController.transform.position = hit.point + (Vector3.up * 0);
                            _controller.enabled = true;
                            Camera.main.GetComponent<CS_CameraUtilities>().Shake(amplitude, frequence, timeShake, true, true);
                        }
                    }
                }
            }

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

        lastInputDown = inputDown;
    }

    public void OnBigAttack(CallbackContext context)
    {
        if (this.enabled)
        {
            inputDown = context.ReadValueAsButton();
        }
    }

    private void Dash()
    {
        float finalSpeed = Mathf.Lerp(speed_SideDash, speed_DepthDash, Mathf.Abs(Vector3.Dot(thirdPersonController.transform.forward, new Vector3(-1, 0, 1).normalized)));
        _controller.SimpleMove(_controller.transform.forward * finalSpeed * Time.deltaTime);
    }
}
