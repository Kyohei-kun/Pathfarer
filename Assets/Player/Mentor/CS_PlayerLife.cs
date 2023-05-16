using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_PlayerLife : MonoBehaviour
{
    [SerializeField] CS_Mentor mentor;

    [SerializeField] int lifeMax = 3;
    [Space] [ProgressBar("Life", "lifeMax", EColor.Red)] [SerializeField] int currentLife;

    void Start()
    {
        currentLife = lifeMax;
    }

    [Button]
    private void LoseLife()
    {
        currentLife--;
        currentLife = Mathf.Clamp(currentLife, 0, lifeMax);
        UpdateFXs();
    }

    [Button]
    private void GainLife()
    {
        currentLife++;
        currentLife = Mathf.Clamp(currentLife, 0, lifeMax);
        UpdateFXs();
    }

    private void UpdateFXs()
    {
        mentor.UpdateVisuel_Life(currentLife);
    }
}