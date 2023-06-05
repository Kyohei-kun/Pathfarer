using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CS_F_HeavyAttack;

public interface CS_I_Attackable
{
    void TakeDamage(float damage, PlayerAttackType type);

}
