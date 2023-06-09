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

    static List<string> currentScenesLD = new List<string>();

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
            LoadScene(scene, LoadSceneMode.Additive);
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

    public List<AsyncOperation> UnloadCurrentLDScenes()
    {
        List<AsyncOperation> result = new List<AsyncOperation>();
        foreach (var item in currentScenesLD)
        {
            result.Add(SceneManager.UnloadSceneAsync(item));
        }
        currentScenesLD.Clear();
        return result;
    }

    public List<AsyncOperation> UnloadScenes(List<string> scenes)
    {
        List<AsyncOperation> result = new List<AsyncOperation>();
        foreach (var item in scenes)
        {
            result.Add(SceneManager.UnloadSceneAsync(item));
            if(currentScenesLD.Contains(item))
            {
                currentScenesLD.Remove(item);
            }
        }
        return result;
    }

    public void LoadScenes(List<string> scenes)
    {
        foreach (var item in scenes)
        {
            SceneManager.LoadScene(item, LoadSceneMode.Additive);
            currentScenesLD.Add(item);
        }
    }

    public static void LoadScene(string sceneName, LoadSceneMode mode)
    {
        SceneManager.LoadScene(sceneName, mode);
        currentScenesLD.Add(sceneName);
    }

    public static void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
        currentScenesLD.Remove(sceneName);
    }
}
