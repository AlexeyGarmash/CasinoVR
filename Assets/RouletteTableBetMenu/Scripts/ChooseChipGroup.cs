using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseChipGroup : MonoBehaviour
{
    [SerializeField] private string _chipResourceName;
    [SerializeField] private GameObject _chipPrefab;
    [SerializeField] private Button _buttonIncreaseChipCount;
    [SerializeField] private Button _buttonDecreaseChipCount;
    [SerializeField] private TMP_Text _textChipCost;
    [SerializeField] private TMP_Text _textChipCount;
    [SerializeField] private int _chipCost;

    private int _currentChipCount = 0;
    private Action<int, bool> _onChipCountChanged;

    public int TotalCost
    {
        get => _currentChipCount * _chipCost;
    }

    public string ChipResourceName { get => _chipResourceName; }
    public int CurrentChipCount { get => _currentChipCount; }
    public GameObject ChipPrefab { get => _chipPrefab; }

    public Action<int, bool> OnChipCountChanged
    {
        get => _onChipCountChanged;
        set
        {
            _onChipCountChanged = value;
        }
    }


    private void Awake()
    {
        _buttonIncreaseChipCount.onClick.AddListener(OnButtonIncreaseCount_Click);
        _buttonDecreaseChipCount.onClick.AddListener(OnButtonDecreaseCount_Click);
    }

    private void Start()
    {
        SetTextCurrentChipsCount(_currentChipCount);
        SetTextChipsCost(_chipCost);
    }

    private void OnButtonIncreaseCount_Click()
    {
        _currentChipCount++;
        SetTextCurrentChipsCount(_currentChipCount);
        _onChipCountChanged?.Invoke(_chipCost, true);
    }

    private void OnButtonDecreaseCount_Click()
    {
        if (_currentChipCount != 0)
        {
            _currentChipCount--;
            SetTextCurrentChipsCount(_currentChipCount);
            _onChipCountChanged?.Invoke(_chipCost, false);
        }
    }

    private void SetTextCurrentChipsCount(int count)
    {
        _textChipCount.text = count.ToString();
    }

    private void SetTextChipsCost(int cost)
    {
        _textChipCost.text = string.Format("{0}$", cost);
    }

    public void ClearUI()
    {
        _currentChipCount = 0;
        SetTextCurrentChipsCount(_currentChipCount);
    }
    
}
