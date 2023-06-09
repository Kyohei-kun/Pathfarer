using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_GameManager : MonoBehaviour
{
    Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnDestroy()
    {
        CS_TriggerMerger.Initialisation();
    }

    public void OnPlayerDeath()
    {
        StartCoroutine(Co_OnDeath());
    }

    private IEnumerator Co_OnDeath()
    {
        yield return new WaitForSeconds(0.8f);

        CS_CameraFade cam = Camera.main.GetComponent<CS_CameraFade>();
        cam.FadeIn();

        while (cam.InFade()) { yield return null; }

        if (CS_Checkpoints.actualCheckpoint != null)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.position = CS_Checkpoints.actualCheckpoint.transform.position + (Vector3.up * 0.3f);
            player.GetComponent<CharacterController>().enabled = true;
        }
        else
        {
            Debug.LogError($"Le joueur n'as actuellement pas de checkpoint activé !\n" +
                $"Ca peut être par ce qu'il meurt avant d'en avoir trigger un, ou a cause d'une erreure de code.\n" +
                $"Nombres de checkpoint connus : {CS_Checkpoints.checkpointsConnus.Count}.");
        }

        //CLOU refill

        player.GetComponent<CS_PlayerLife>().FullLife();

        List<AsyncOperation> unloadOperations = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<CS_SceneManager>().UnloadCurrentLDScenes();

        while(!unloadOperations.TrueForAll(operation => operation.isDone))
        {
            yield return null;
        }

        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<CS_SceneManager>().LoadScenes(CS_Checkpoints.actualCheckpoint.SceneOfCheckpoint);

        cam.FadeOut();
    }
}
