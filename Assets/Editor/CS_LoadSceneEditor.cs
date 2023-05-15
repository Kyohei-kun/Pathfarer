#if (UNITY_EDITOR)
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_LoadSceneEditor : MonoBehaviour
{

    [ReorderableList] public List<SceneAsset> scenes = new List<SceneAsset>();

    [Button("■■■■ Load Scenes ■■■■")]
    public void OpenMainScene()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        foreach (var item in scenes)
        {
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(item), OpenSceneMode.Additive);
        }
    }
}
#endif