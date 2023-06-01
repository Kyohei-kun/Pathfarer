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
    [BoxGroup("Parameter")][SerializeField] Animator animator;
    [BoxGroup("Visuel")][SerializeField] GameObject charge_FX;

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


    private void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (inCharge)
            currentCharge += Time.deltaTime;

        if (inputDown && !lastInputDown)
        {
            if (thirdPersonController.grounded)
            {
                thirdPersonController.CanMove = false;
                charge_FX.SetActive(true);
                currentCharge = 0;
                inCharge = true;
                CS_VibrationControler.SetVibration(0.5f, 0.2f, 1.5f);
                animator.SetTrigger("HeavyAttack");
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
                            GameObject speedPlane = GameObject.Instantiate(prefab_SpeedPlane);
                            GameObject fx_Ground = GameObject.Instantiate(prefab_FxGround);
                            CS_VibrationControler.SetVibration(10, 1, 0.4f);
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
            charge_FX.SetActive(false);
            inCharge = false;
            
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

    public void CanMove()
    {
        thirdPersonController.CanMove = true;
    }

    private void Dash()
    {
        //float finalSpeed = Mathf.Lerp(speed_SideDash, speed_DepthDash, Mathf.Abs(Vector3.Dot(thirdPersonController.transform.forward, new Vector3(-1, 0, 1).normalized)));
        //_controller.SimpleMove(_controller.transform.forward * finalSpeed * Time.deltaTime);
    }

    public enum PlayerAttackType
    {
        Simple,
        Heavy,
        Pilon
    }
}

