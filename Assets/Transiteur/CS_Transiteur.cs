using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_Transiteur : MonoBehaviour
{
    [BoxGroup("Scenes")][SerializeField][Scene][ReorderableList] List<string> scenesToLoad;
    [BoxGroup("Scenes")][SerializeField][Scene][ReorderableList] List<string> scenesToUnload;
    [SerializeField] Transform socketPlayerTP;

    static bool drawGizmo;

    [Button][HideIf("drawGizmo")] public void DrawGizmo() { drawGizmo = true; }
    [Button][ShowIf("drawGizmo")] public void HideGizmo() { drawGizmo = false; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           StartCoroutine(Transition(other.gameObject));
        }
    }

    private IEnumerator Transition(GameObject player)
    {
        CS_CameraFade FadeUtilitie = Camera.main.GetComponent<CS_CameraFade>();
        FadeUtilitie.FadeIn();
        player.GetComponent<CharacterController>().enabled = false;

        while (FadeUtilitie.InFade()) { yield return null; }

        foreach (string nameScene in scenesToLoad) { SceneManager.LoadScene(nameScene, LoadSceneMode.Additive); }

        //TP
        player.gameObject.transform.position = socketPlayerTP.position;
        player.gameObject.GetComponent<CharacterController>().enabled = true;

        foreach (string nameScene in scenesToUnload) { SceneManager.UnloadSceneAsync(nameScene); }

        FadeUtilitie.FadeOut();
    }

    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = new Color(0.3f, 0f, 0.9f, 0.5f);
            Gizmos.DrawCube(transform.position, transform.localScale);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(socketPlayerTP.position, 0.3f);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(socketPlayerTP.position, transform.position);
        }
    }
}