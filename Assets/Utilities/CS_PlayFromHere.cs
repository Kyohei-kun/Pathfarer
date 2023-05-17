using StarterAssets;
using System.Collections;
using System.Collections.Generic;
#if(UNITY_EDITOR)
using UnityEditor;
using UnityEngine;

public static class CS_PlayFromHere
{
    static Vector3 playerPosition = Vector3.zero;

    public static Vector3 PlayerPosition { get => playerPosition; set => playerPosition = value; }

    [MenuItem("PlayFromHere/Special Command %g")]
    static void SpecialCommand()
    {
        //Canvas.ForceUpdateCanvases();
        //SceneView.RepaintAll();
        //EditorWindow.GetWindow<SceneView>().Repaint();

        //HandleUtility.Repaint();
        if (SceneView.sceneViews.Count > 0)
        {
            SceneView sceneView = (SceneView)SceneView.sceneViews[0];
            sceneView.Focus();
        }

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                playerPosition = hit.point;
                Debug.LogWarning(playerPosition);
                //GameObject.FindGameObjectWithTag("Player").transform.position = playerPosition;

                EditorApplication.EnterPlaymode();
            }
        }
    }
}
#endif