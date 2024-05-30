using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Image unitHPBar;

    private IUnit unit;

    void Start()
    {
        unit = gameObject.GetComponentInParent<IUnit>();
        unit.onUnitHealthChange += UpdateHpUI;
    }

    void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, 
            mainCamera.transform.rotation * Vector3.up);
    }

    private void UpdateHpUI(int unitCurrentHealth)
    {
        unitHPBar.fillAmount = unit.unitHealthCurrent / (float) unit.unitData.unitHealthMax;
    }
}
