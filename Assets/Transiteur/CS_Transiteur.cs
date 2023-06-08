using NaughtyAttributes;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
            //FadeOUT
            foreach (string nameScene in scenesToLoad) { SceneManager.LoadScene(nameScene, LoadSceneMode.Additive); }

            //TP
            other.gameObject.GetComponent<CharacterController>().enabled = false;
            other.gameObject.transform.position = socketPlayerTP.position;
            other.gameObject.GetComponent<CharacterController>().enabled = true;

            foreach (string nameScene in scenesToUnload) { SceneManager.UnloadSceneAsync(nameScene); }
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(socketPlayerTP.position, 0.3f);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(socketPlayerTP.position, transform.position);
        }
    }
}