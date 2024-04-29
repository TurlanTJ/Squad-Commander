using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance { get; private set;}
    void Awake() {
        if(instance != null)
            return;
        
        instance = this;
    }

    [SerializeField] private List<GameObject> _availableUnits = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedUnits = new List<GameObject>();

    private List<Vector3> unitSpawnPositions = new List<Vector3>();

    public UnitSelectionManager unitSelectionManager;
    public UnitCommandManager unitCommandManager;

    public delegate void OnUnitSpawn(List<GameObject> spawnedUnits);
    public OnUnitSpawn onUnitSpawn;

    // Start is called before the first frame update
    void Start()
    {
        unitSpawnPositions.Add(new Vector3(-7.5f, 0.5f, 0f));
        unitSpawnPositions.Add(new Vector3(-2.5f, 0.5f, 0f));
        unitSpawnPositions.Add(new Vector3(2.5f, 0.5f, 0f));
        unitSpawnPositions.Add(new Vector3(7.5f, 0.5f, 0f));

        InitiateAllUnits();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitiateAllUnits()
    {
        foreach(GameObject unit in _availableUnits)
        {
            foreach(Vector3 spawnPos in unitSpawnPositions)
            {
                GameObject u = Instantiate(unit, spawnPos, Quaternion.identity);
                spawnedUnits.Add(u);
                u.GetComponent<Unit>().SetUnitOrder(_availableUnits.IndexOf(unit) + 1);

                unitSpawnPositions.Remove(spawnPos);
                break;
            }
        }

        unitSelectionManager.spawnedUnits = spawnedUnits;
        onUnitSpawn?.Invoke(spawnedUnits);
    }

    public List<GameObject> GetSpawnedUnits()
    {
        return spawnedUnits;
    }

    public bool ContainsSelectedUnits()
    {
        return unitSelectionManager.ContainsSelectedUnits();
    }

    public void SelectUnit(GameObject unit, bool shiftSelect)
    {
        unitSelectionManager.SelectUnit(unit, shiftSelect);
    }

    public void DeselectUnit()
    {
        unitSelectionManager.DeselectUnit();
    }

    public void DeselectUnit(GameObject unit)
    {
        unitSelectionManager.DeselectUnit(unit);
    }

    public void FocusOnUnit(GameObject unit)
    {
        unitSelectionManager.FocusOnUnit(unit);
    }

    public void FocusOnUnit(int unitOrder)
    {
        unitSelectionManager.FocusOnUnit(unitOrder);
    }

    public void MoveSelectedUnits(Vector3 destination)
    {
        unitCommandManager.movementManager.MoveSelectedUnits(destination);
    }

    public void StopSelectedUnitsMovement()
    {
        unitCommandManager.movementManager.StopSelectedUnitsMovement();
    }

    public void InitiateAttack(GameObject target)
    {
        unitCommandManager.attackManager.InitiateAttack(target);
    }
}
