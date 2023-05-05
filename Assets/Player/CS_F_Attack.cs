using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

enum RightLeft
{
    Right = -1,
    Left = 1
}

public class CS_F_Attack : MonoBehaviour
{
    bool inputDown;
    bool lastInputDown = false;
    [SerializeField] Image imageSlashR;
    [SerializeField] Image imageSlashL;

    RightLeft lastAttackDirection = RightLeft.Left;
    float nbFrameBeforeLastAttack = 0;
    bool lastAttackIsFinish = true;

    public void OnAttack(CallbackContext context)
    {
        inputDown = context.ReadValueAsButton();
    }

    private void Update()
    {
        nbFrameBeforeLastAttack++;

        if (nbFrameBeforeLastAttack > 20)
        {
            lastAttackIsFinish = true;
            imageSlashL.enabled = false;
            imageSlashR.enabled = false;
            nbFrameBeforeLastAttack = 0;
        }

        if (inputDown && !lastInputDown && lastAttackIsFinish)
        {
            if (lastAttackDirection == RightLeft.Right)
                imageSlashL.enabled = true;
            else
                imageSlashR.enabled = true;

            lastAttackIsFinish = false;
            lastAttackDirection = (RightLeft)((int)lastAttackDirection * -1);
        }


        lastInputDown = inputDown;
    }
}
