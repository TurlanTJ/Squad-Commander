using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject unitInfoPanelParent;
    [SerializeField] private GameObject unitInfoPanel;
    [SerializeField] private GameObject unitIconPanelParent;
    [SerializeField] private GameObject unitIconPanel;

    private UnitManager unitManager;
    // private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        unitManager = UnitManager.instance;
        // playerManager = PlayerManager.instance;

        unitManager.onUnitSpawn += InstantiateUnitPanels;
        unitManager.onUnitSpawn += InstantiateUnitIconPanels;
    }

    private void InstantiateUnitPanels(List<GameObject> spawnedUnits)
    {
       // UnitInfoPanel Instantiation Logic
        unitInfoPanelParent.GetComponent<GridLayoutGroup>().enabled = false;
        foreach(GameObject unit in spawnedUnits)
        {
            GameObject infoPanel = Instantiate(unitInfoPanel, unitInfoPanelParent.transform);
            infoPanel.GetComponent<UnitInfoPanel>().InitializeInfoPanel(unit.GetComponent<Unit>());
        }
        unitInfoPanelParent.GetComponent<GridLayoutGroup>().enabled = true;
    }

    private void InstantiateUnitIconPanels(List<GameObject> spawnedUnits)
    {
        unitIconPanelParent.GetComponent<GridLayoutGroup>().enabled = false;
        foreach(GameObject unit in spawnedUnits)
        {
            GameObject iconPanel = Instantiate(unitIconPanel, unitIconPanelParent.transform);
            iconPanel.GetComponent<UnitIconPanel>().InitializeIconPanel(unit);
        }
        unitIconPanelParent.GetComponent<GridLayoutGroup>().enabled = true;
    }
}
