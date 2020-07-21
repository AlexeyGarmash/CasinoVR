using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyChipMenu : MonoBehaviour
{
    [SerializeField] private ChipsSpawner _chipsSpawner;
    [SerializeField] private List<ChooseChipGroup> _chooseChipGroups;
    [SerializeField] private TMP_Text _textCurrentSpentCost;
    [SerializeField] private TMP_Text _textAllMoney;
    [SerializeField] private Button _buttonBuyChips;
    [SerializeField] private Button _buttonCloseMenu;
    

    private int _totalCurrentSpentCost = 0;

    public int AllMoney = 50000;

    private void Awake()
    {
        foreach (var choiceGroup in _chooseChipGroups)
        {
            choiceGroup.OnChipCountChanged += OnChipCountChanged;
        }
        _buttonBuyChips.onClick.AddListener(OnButtonBuyChips_Click);
        _buttonCloseMenu.onClick.AddListener(OnButtonCloseMenu_Click);
    }

    

    private void Start()
    {
        gameObject.SetScale(Vector3.zero);
        OpenMenu();
    }

    public void SetAllMoney(int money)
    {
        AllMoney = money;
    }

    private void OnChipCountChanged(int cost, bool increase)
    {
        _totalCurrentSpentCost += increase ? cost : -cost;
        SetCurrentSpentCost(_totalCurrentSpentCost);
    }

    private void OnButtonBuyChips_Click()
    {
        BuyChips();
    }

    private void OnButtonCloseMenu_Click()
    {
        CloseMenu();
    }

    public void CloseMenu()
    {
        _totalCurrentSpentCost = 0;
        SetCurrentSpentCost(_totalCurrentSpentCost);
        ClearChoiceGroups();
        gameObject.SetScale(Vector3.zero);
    }

    public void OpenMenu()
    {
        SetCurrentSpentCost(_totalCurrentSpentCost);
        SetAllMoneyText(AllMoney);
        gameObject.SetScale(Vector3.one);
    }

    private void BuyChips()
    {
        if(_totalCurrentSpentCost > AllMoney)
        {
            print("Too much chips!");
        }
        else
        {
            AllMoney -= _totalCurrentSpentCost;
            SetAllMoneyText(AllMoney);
            _totalCurrentSpentCost = 0;
            SetCurrentSpentCost(_totalCurrentSpentCost);
            SentChipsToSpawn();
            ClearChoiceGroups();
        }
    }

    private void SentChipsToSpawn()
    {
        _chipsSpawner.RecieveAndSpawnBoughtChips(_chooseChipGroups, false);
    }

    private void ClearChoiceGroups()
    {
        foreach (var group in _chooseChipGroups)
        {
            group.ClearUI();
        }
    }

    private void SetCurrentSpentCost(int cost)
    {
        if (cost == 0)
        {
            _textCurrentSpentCost.text = "";
        }
        else
        {
            _textCurrentSpentCost.text = string.Format("-{0}$", cost);
        }
    }

    private void SetAllMoneyText(int allMoney)
    {
        _textAllMoney.text = string.Format("{0}$", allMoney);
    }

    private void OnDisable()
    {
        foreach (var choiceGroup in _chooseChipGroups)
        {
            choiceGroup.OnChipCountChanged -= OnChipCountChanged;
        }
    }
}
