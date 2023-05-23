using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_PlayerLife : MonoBehaviour
{
    [SerializeField] CS_F_Mentor mentor;

    [SerializeField] int lifeMax = 3;
    [Space] [ProgressBar("Life", "lifeMax", EColor.Red)] [SerializeField] int currentLife;

    void Start()
    {
        currentLife = lifeMax;
    }

    [Button]
    public void LoseLife()
    {
        currentLife--;
        currentLife = Mathf.Clamp(currentLife, 0, lifeMax);
        UpdateFXs();
    }

    [Button]
    public void GainLife()
    {
        currentLife++;
        currentLife = Mathf.Clamp(currentLife, 0, lifeMax);
        UpdateFXs();
    }

    private void UpdateFXs()
    {
        mentor.UpdateVisuel_Life(currentLife);
    }

    // Update temporaire pour boutton clavier !
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            GoToCheckpoint();
        }
    }

    [Button]
    void GoToCheckpoint()
    {
        if (CS_Checkpoints.actualCheckpoint != null)
        {
            GetComponent<CharacterController>().enabled = false;

            transform.position = CS_Checkpoints.actualCheckpoint.transform.position + (Vector3.up * 0.3f);

            GetComponent<CharacterController>().enabled = true;
        }
        else
        {
            Debug.LogError($"Le joueur n'as actuellement pas de checkpoint activé !\n" +
                $"Ca peut être par ce qu'il meurt avant d'en avoir trigger un, ou a cause d'une erreure de code.\n" +
                $"Nombres de checkpoint connus : {CS_Checkpoints.checkpointsConnus.Count}.");
        }
    }
}