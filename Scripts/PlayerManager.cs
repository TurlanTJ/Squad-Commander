using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }
    void Awake()
    {
        if(instance != null)
            return;
        
        instance = this;
    }

    public Vector3 mousePositionCurrent { get; private set; }
    public Vector3 mousePositionLeftClick { get; private set; }
    public Vector3 mousePositionRightClick { get; private set; }

    private Camera mainCamera;
    private GameObject hoveredGameObject;

    private UnitManager unitManager;

    public bool isGivingCommand = false;

    void Start()
    {
        mainCamera = Camera.main;
        unitManager = UnitManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        HanldeMouseHover();
        HandleSelection();
        HandleUnitCommand();
    }

    public void HanldeMouseHover()
    {
        mousePositionCurrent = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            hoveredGameObject = hit.collider.gameObject;
        }
    }

    public void HandleSelection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.collider.gameObject.TryGetComponent(out Interactable interactable))
                {
                    InteractableType type = interactable.interactableType;

                    if(type == InteractableType.Unit)
                    {
                        if(Input.GetKey(KeyCode.LeftShift))
                            unitManager.SelectUnit(hit.collider.gameObject, true);
                        else
                            unitManager.SelectUnit(hit.collider.gameObject, false);
                    }
                    else if(type == InteractableType.Ground)
                    {
                        if(unitManager.ContainsSelectedUnits())
                            unitManager.DeselectUnit();
                    }
                }
            }
        }
    }

    public void HandleUnitCommand()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if(isGivingCommand)
                return;
            if(!unitManager.ContainsSelectedUnits())
                return;

            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.collider.gameObject.TryGetComponent(out Interactable interactable))
                {
                    InteractableType type = interactable.interactableType;

                    if(type == InteractableType.Ground)
                    {
                        if(unitManager.unitSelectionManager.ContainsSelectedUnits())
                            unitManager.unitCommandManager.movementManager.MoveSelectedUnits(hit.point);
                    }

                    if(type == InteractableType.Unit)
                    {
                        if(interactable.gameObject.GetComponent<IUnit>().unitFaction == UnitFaction.Enemy)
                        {
                            if(unitManager.unitSelectionManager.ContainsSelectedUnits())
                                unitManager.unitCommandManager.attackManager.InitiateAttack(interactable.gameObject);
                        }
                    }
                }
            }
        }
    }
}
