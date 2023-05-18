using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CS_F_FireFlies : MonoBehaviour
{
    GameObject player;
    bool snaped = false;

    //Input
    private Vector2 move;

    //Movment
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] float thresholdUnSnap = 0.4f;
    [SerializeField] Light lightFireflies_Snapped;

    private float _speed;
    private bool grounded = true;
    float GroundedOffset = -0.14f;
    float GroundedRadius = 0.28f;
    LayerMask GroundLayers;
    float _targetRotation = 0.0f;
    private GameObject _mainCamera;
    float RotationSmoothTime = 0.12f;
    private CharacterController _controller;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    private float _rotationVelocity;

    float _jumpTimeoutDelta;
    float _fallTimeoutDelta;
    float FallTimeout = 0.15f;
    float JumpTimeout = 0.50f;

    bool featureUnlocked;
    GameObject childGeometric;

    public void LockFeature()
    {
        lightFireflies_Snapped.enabled = false;
        featureUnlocked = false;
        try
        {
            childGeometric.SetActive(false);
        }
        catch (System.Exception){}
    }

    public void UnlockFeature()
    {
        GetComponent<Renderer>().enabled = true;
        lightFireflies_Snapped.enabled = true;
        featureUnlocked = true;
        try
        {
            childGeometric.SetActive(true);
        }
        catch (System.Exception){}
        Recall();
    }

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {
        childGeometric = transform.GetChild(0).gameObject;
        _controller = GetComponent<CharacterController>();
        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (featureUnlocked)
        {
            if (snaped)
            {
                transform.position = player.transform.position;
            }
            else
            {
                JumpAndGravity();
                GroundedCheck();
                Move();
            }
        }
    }

    public void OnMoveFireFlies(CallbackContext context)
    {
        if (featureUnlocked)
        {
            Vector2 tempMove = context.ReadValue<Vector2>();
            if (snaped)
            {
                if (tempMove.magnitude > thresholdUnSnap)
                {
                    snaped = false;
                    _controller.enabled = true;
                    move = tempMove;
                    lightFireflies_Snapped.enabled = false;
                }
            }
            else
            {
                _controller.enabled = true;
                move = tempMove;
            }
        }
    }

    public void OnRecall(CallbackContext context)
    {
        if (context.ReadValueAsButton() && featureUnlocked)
        {
            Recall();
        }
    }

    private void Recall()
    {
        _controller.enabled = false;
        transform.position = player.transform.position;
        snaped = true;
        lightFireflies_Snapped.enabled = true;
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }

    private void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = moveSpeed;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (move == Vector2.zero) targetSpeed = 0.0f;



        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.y).magnitude;

        float speedOffset = 0.1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            //_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * move.magnitude, Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            //_speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            //_speed = targetSpeed;
        }

        _speed = targetSpeed * move.magnitude;

        // normalise input direction
        Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        targetDirection = targetDirection.normalized;

        // move the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private void JumpAndGravity()
    {
        if (grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += gravity * Time.deltaTime;
        }
    }
}
