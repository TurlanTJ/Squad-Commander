using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private List<Scenes> allScenes = new List<Scenes>();

    public int currentScene;

    public void LoadScene(int sceneIdx)
    {
        if(sceneIdx < 0)
            return;
        if(sceneIdx >= allScenes.Count)
        {
            LoadMainMenu();
            return;
        }

        SceneManager.LoadScene(allScenes[sceneIdx].ToString());
        currentScene = sceneIdx;
    }

    public void LoadMainMenu()
    {
        LoadScene(0);
    }

    public void LoadTutorial()
    {
        LoadScene(1);
    }

    public void LoadCurrentScene()
    {
        LoadScene(currentScene);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}

public enum Scenes
{
    MainMenuScene,
    DemoScene
}
