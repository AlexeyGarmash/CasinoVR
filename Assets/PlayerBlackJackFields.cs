using Assets.Scipts.BackJack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackJackFields : MonoBehaviour
{
    public ChipsField bettingField { get; private set; }
    public BlackJackPlayerCardField blackJackField { get; private set; }
    void Start()
    {
        bettingField = GetComponentInChildren<ChipsField>();
        blackJackField = GetComponentInChildren<BlackJackPlayerCardField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
