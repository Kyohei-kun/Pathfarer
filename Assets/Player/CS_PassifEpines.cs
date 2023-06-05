using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CS_F_HeavyAttack;

public class CS_PassifEpines : MonoBehaviour
{
    private bool isActif = false;
    [SerializeField] float epinesDmg = 1;
    [SerializeField][Range(0.5f, 5)] float radiusEpine = 3f;

    public bool IsActif { get => isActif; set => isActif = value; }

    [Button]
    public void Dmg()
    {
        if(isActif)
        {
            List<Collider> colliders = Physics.OverlapSphere(transform.position, radiusEpine).ToList();
            foreach(Collider collider in colliders)
            {
                CS_I_Attackable attackable = collider.GetComponent<CS_I_Attackable>();
                if (attackable != null)
                {
                    attackable.TakeDamage(epinesDmg, PlayerAttackType.Epines);
                }
            }
        }
    }
}
