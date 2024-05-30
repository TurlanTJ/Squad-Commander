using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjective : MonoBehaviour
{
    public static LevelObjective instance;
    void Awake()
    {
        if(instance != null)
            return;
        instance = this;
    }

    [SerializeField] private List<IUnit> allEnemies = new List<IUnit>();

    [SerializeField] private GameObject levelEndPanel;

    // Start is called before the first frame update
    void Start()
    {
        OpenLevelClearPanel(false);
        foreach(IUnit enemy in allEnemies)
            enemy.onUnitDeath += RemoveDeadEnemy;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            OpenLevelClearPanel(!levelEndPanel.activeSelf);
    }

    public void OpenLevelClearPanel(bool sts)
    {
        levelEndPanel.SetActive(sts);
    }

    public void LoadCurrentScene()
    {
        LevelManager.instance.LoadCurrentScene();
    }

    public void LoadNextScene()
    {
        LevelManager.instance.LoadScene(LevelManager.instance.currentScene + 1);
    }

    public void LoadMainMenu()
    {
        LevelManager.instance.LoadMainMenu();
    }

    private void RemoveDeadEnemy(IUnit enemy)
    {
        allEnemies.Remove(enemy);

        if(allEnemies.Count <= 0)
            OpenLevelClearPanel(true);
    }
}
