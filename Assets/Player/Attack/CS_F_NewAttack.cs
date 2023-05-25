using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CS_F_NewAttack : MonoBehaviour
{
    [SerializeField] Animator animator;

    bool inputDown;
    bool lastInputDown;

    bool acceptInput = true;
    int animatorState = 0;

    public void OnAttack(CallbackContext context)
    {
        if (this.enabled)
        {
            inputDown = context.ReadValueAsButton();
        }
    }

    private void Update()
    {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Neutral") == animator.GetCurrentAnimatorStateInfo(0).IsName("Anim_PlayerAttack_1"));
        if (inputDown && !lastInputDown)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Neutral"))//Si on a pas de combo commencé
            {
                animator.SetInteger("State", 1);
                animatorState = 1;
                Debug.Log("Attack");
            }
        }

        inputDown = lastInputDown;
    }

    public void SetAnimatorState(int i)
    {
        animator.SetInteger("State", i);
        animatorState = i;
    }

    public void AcceptInput(int i)
    {
        acceptInput = i==1?true:false;
    }
}
