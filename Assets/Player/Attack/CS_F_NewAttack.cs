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

    public void OnAttack(CallbackContext context)
    {
        if (this.enabled)
        {
            inputDown = context.ReadValueAsButton();
        }
    }

    private void Update()
    {
        Debug.Log(LastInfoAnim);
        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Neutral") == animator.GetCurrentAnimatorStateInfo(0).IsName("Anim_PlayerAttack_1"));
        if (inputDown && !lastInputDown)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Anim_PlayerAttack_0 0"))
            {
                animator.SetBool("InputTempo", true);
            }

            //switch (LastInfoAnim)
            //{
            //    case AnimationAttackState.Neutral:
            //        animator.SetInteger("State", 1);
            //        break;
            //    case AnimationAttackState.Start_1:
            //        break;
            //    case AnimationAttackState.Mid_1:
            //        animator.SetInteger("State", 2);
            //        break;
            //    case AnimationAttackState.End_1:
            //        break;
            //    case AnimationAttackState.Start_2:
            //        break;
            //    case AnimationAttackState.Mid_2:
            //        animator.SetInteger("State", 3);
            //        break;
            //    case AnimationAttackState.End_2:
            //        break;
            //    case AnimationAttackState.Start_3:
            //        break;
            //    case AnimationAttackState.Mid_3:
            //        break;
            //    case AnimationAttackState.End_3:
            //        break;
            //    default:
            //        break;
            //}
        }
        else
        {
            //switch (LastInfoAnim)
            //{
            //    case AnimationAttackState.Neutral:
            //        break;
            //    case AnimationAttackState.Start_1:
            //        break;
            //    case AnimationAttackState.Mid_1:
            //        break;
            //    case AnimationAttackState.End_1:
            //        InfoAnimation(AnimationAttackState.Neutral);
            //        break;
            //    case AnimationAttackState.Start_2:
            //        break;
            //    case AnimationAttackState.Mid_2:
            //        break;
            //    case AnimationAttackState.End_2:
            //        InfoAnimation(AnimationAttackState.Neutral);
            //        break;
            //    case AnimationAttackState.Start_3:
            //        break;
            //    case AnimationAttackState.Mid_3:
            //        break;
            //    case AnimationAttackState.End_3:
            //        InfoAnimation(AnimationAttackState.Neutral);
            //        break;
            //    default:
            //        break;
            //}
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

    IEnumerator ListenInput(float time)
    {
        demandedInput = false;
        animator.SetBool("InputTempo", false);
        while (time > 0)
        {
            if (inputDown && !lastInputDown)
            {
                demandedInput = true;
                animator.SetBool("InputTempo", true);
            }
            yield return 0;
        }
    }
}
