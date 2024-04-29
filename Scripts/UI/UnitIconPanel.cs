using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitIconPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject unitIcon;
    [SerializeField] private GameObject unitHighlighter;
    [SerializeField] private GameObject unitHealthBar;
    [SerializeField] private GameObject unitEnergyBar;

    private GameObject unitPrefab;
    private Unit unit;

    public void InitializeIconPanel(GameObject u)
    {
        HighlightPanel(false);
        unitPrefab = u;
        unit = u.GetComponent<Unit>();
        unitIcon.GetComponent<Image>().sprite = unit.unitData.unitIcon;

        unit.onUnitSelection += HighlightPanel;
        unit.onUnitHealthChange += UpdateUnitHealth;
        unit.onUnitEnergyChange += UpdateUnitEnergy;
    }

    private void HighlightPanel(bool sts)
    {
        unitHighlighter.SetActive(sts);
    }

    private void UpdateUnitHealth(int unitCurrentHealth)
    {
        unitHealthBar.GetComponent<Image>().fillAmount = unitCurrentHealth / (float)unit.unitData.unitHealthMax;
    }

    private void UpdateUnitEnergy(int unitCurrentEnergy)
    {
        unitHealthBar.GetComponent<Image>().fillAmount = unitCurrentEnergy / (float)unit.unitData.unitEnergyMax;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UnitManager.instance.SelectUnit(unitPrefab, false);
    }
}
