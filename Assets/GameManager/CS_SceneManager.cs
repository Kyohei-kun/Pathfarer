using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_SceneManager : MonoBehaviour
{
    [Scene][SerializeField] string menuScene;
    [Scene][SerializeField] string coreScene;

    [Scene][ReorderableList] public List<string> startScenes_LD = new List<string>();

    private void Start()
    {
        if (GameObject.FindGameObjectsWithTag("SceneManager").Count() > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Load all scenes to start game
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(coreScene, LoadSceneMode.Single);

        SetActiveCoreScene();
        foreach (var scene in startScenes_LD)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        }
    }

    private async void SetActiveCoreScene()
    {
        await System.Threading.Tasks.Task.Delay((int)(1000));
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(coreScene));
    }
    public void StartGameWithDelay()
    {
        Invoke("StartGame", 1.5f);
    }
}
