using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject unitDeathPanel;
    [SerializeField] private GameObject unitHighlighPanel;
    [SerializeField] private GameObject unitHealthBarCurrent;
    [SerializeField] private GameObject unitHealthBarCurrentText;
    [SerializeField] private GameObject unitEnergyBarCurrent;
    [SerializeField] private GameObject unitEnergyBarCurrentText;
    [SerializeField] private GameObject unitIcon;
    [SerializeField] private Image abilityCD;

    private UnitManager unitManager;
    private Unit unit;

    private bool isPanelFocused = false;

    void Start()
    {
        unitManager = UnitManager.instance;
    }

    public void InitializeInfoPanel(Unit newUnit)
    {
        unitDeathPanel.SetActive(false);
        FocusPanel(false);
        OpenPanel(false);

        unit = newUnit;

        // Subscribe To Delegates
        unit.onUnitSelection += OpenPanel;
        unit.onUnitFocused += FocusPanel;
        unit.onUnitHealthChange += UpdateUnitHealth;
        unit.onUnitEnergyChange += UpdateUnitEnergy;
        unit.onUnitDeath +=  UpdateUnitDeathStatus;

        unit.unitAbilityManager.onAbilityCDChanged += UpdateUIAbilityCD;
        abilityCD.gameObject.SetActive(false);

        unitIcon.GetComponent<Image>().sprite = unit.unitData.unitIcon;
        unitHealthBarCurrentText.GetComponent<TextMeshProUGUI>().text = $"{unit.GetHealth().x} / {unit.GetHealth().y}";
        unitEnergyBarCurrentText.GetComponent<TextMeshProUGUI>().text = $"{unit.GetEnergy().x} / {unit.GetEnergy().y}";
    }

    public void OpenPanel(bool unitSelected)
    {
        gameObject.SetActive(unitSelected);
    }

    public void FocusPanel(bool status)
    {
        isPanelFocused = status;
        unitHighlighPanel.SetActive(status);
    }

    public void UpdateUnitHealth(int unitCurrentHealth)
    {
        int maxHealth = unit.GetHealth().y;
        TextMeshProUGUI healthText = unitHealthBarCurrentText.GetComponentInChildren<TextMeshProUGUI>();
        healthText.text = $"{unitCurrentHealth} / {maxHealth}";

        unitHealthBarCurrent.GetComponent<Image>().fillAmount = unitCurrentHealth / (float)maxHealth;
    }

    public void UpdateUnitEnergy(int unitCurrentEnergy)
    {
        int maxEnergy = unit.GetEnergy().y;
        TextMeshProUGUI energyText = unitEnergyBarCurrentText.GetComponentInChildren<TextMeshProUGUI>();
        energyText.text = $"{unitCurrentEnergy} / {maxEnergy}";

        unitEnergyBarCurrent.GetComponent<Image>().fillAmount = unitCurrentEnergy / (float)maxEnergy;
    }

    public void UpdateUnitDeathStatus(IUnit unit)
    {
        unitDeathPanel.SetActive(true);
        OpenPanel(false);
    }

    public void UpdateUIAbilityCD(float cd)
    {
        if(cd <= 0f)
            abilityCD.gameObject.SetActive(false);
        else
            abilityCD.gameObject.SetActive(true);

        abilityCD.fillAmount = cd;
    }

    public void UseAbility()
    {
        if(!isPanelFocused)
            return;

        unitManager.unitCommandManager.UseUnitAbility();
    }

    public void Attack()
    {
        if(!isPanelFocused)
            return;

        unitManager.unitCommandManager.AttackUnitTarget();
    }

    public void MoveUnit()
    {
        if(!isPanelFocused)
            return;

        unitManager.unitCommandManager.MoveUnit();
    }

    public void StopUnit()
    {
        if(!isPanelFocused)
            return;

        unitManager.unitCommandManager.StopAllUnitActions();
    }
}
