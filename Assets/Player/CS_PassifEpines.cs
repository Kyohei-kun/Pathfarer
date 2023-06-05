using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CS_PassifEpines : MonoBehaviour
{
    private bool isActif = false;
    private List<CS_Enemy> nearEnnemies = new ();
    [SerializeField] float epinesDmg = 1;
    [SerializeField][MinMaxSlider(0.5f, 5)] float radiusEpine = 3f;

    [Button]
    public void Dmg()
    {
        if(isActif)
        {
            List<Collider> colliders = Physics.OverlapSphere(transform.position, radiusEpine).ToList();
            foreach(Collider collider in colliders)
            {
                CS_I_Attackable attackable = collider.GetComponent<CS_I_Attackable>();
                if ( attackable != null)
                {

                }
            }
        }
    }
}
