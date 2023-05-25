using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CS_AnimationEnum;
using static UnityEngine.InputSystem.InputAction;

public class CS_F_NewAttack : MonoBehaviour
{
    [SerializeField] Animator animator;

    bool inputDown;
    bool lastInputDown;

    bool demandedInput = false;
    AnimationAttackState LastInfoAnim;
    float currentTime = 0;
    float targetTime;

    public bool DemandedInput
    {
        get { return demandedInput; }
        set { demandedInput = value; animator.SetBool("InputTempo", demandedInput); }
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
        currentTime += Time.deltaTime;

        Debug.Log(LastInfoAnim);
        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Neutral") == animator.GetCurrentAnimatorStateInfo(0).IsName("Anim_PlayerAttack_1"));
        if (inputDown && !lastInputDown)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Anim_PlayerAttack_0 0"))
            {
                animator.SetBool("InputTempo", true);
            }
        }

        if (currentTime < targetTime)
        {
            if (inputDown && !lastInputDown)
            {
                DemandedInput = true;
            }
        }

        inputDown = lastInputDown;
    }

    public void SetState(int state)
    {
        animator.SetInteger("State", state);
    }

    public void InfoAnimation(AnimationAttackState animState)
    {
        LastInfoAnim = animState;
    }

    public void ListenInput(float timing)
    {
        DemandedInput = false;
        targetTime = timing;
        currentTime = 0;
    }
}
