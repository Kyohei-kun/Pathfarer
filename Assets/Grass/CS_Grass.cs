using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CS_Grass : MonoBehaviour, CS_I_Attackable
{
    [SerializeField] GameObject pickUpHeart;

    [Header("Drop Rules")][HorizontalLine]
    [SerializeField] bool forceDrop;
    [HideIf("forceDrop")][Range(0, 100)][SerializeField] float tauxDrop;

    [Header("Visuels")][HorizontalLine]
    [SerializeField] List<GameObject> myGrass;
    [SerializeField] VisualEffect fxGrass;
    [ShowIf("forceDrop")][SerializeField] List<GameObject> myFlowers;
    [ShowIf("forceDrop")][SerializeField] VisualEffect fxFlower;

    public void TakeDamage(float damage, CS_F_HeavyAttack.PlayerAttackType type)
    {
        Cutted();
    }

    void Cutted()
    {
        fxGrass.Play();

        for (int i = 0; i < myGrass.Count; i++)
        {
            myGrass[i].transform.localScale = new Vector3(1, 0.1f, 1);
        }

        GetComponent<CapsuleCollider>().enabled = false;

        if (forceDrop)
        {
            fxFlower.Play();
            Instantiate(pickUpHeart, transform.position + Vector3.up, Quaternion.identity);

            for (int i = 0; i < myFlowers.Count; i++)
            {
                Destroy(myFlowers[i]);
            }
        }
        else if (Random.Range(0, 100) <= tauxDrop)
        {
            Instantiate(pickUpHeart, transform.position + Vector3.up, Quaternion.identity);
        }
    }



}
