using Assets.Scipts.BackJack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackJackFields : MonoBehaviour
{

    [SerializeField]
    public ChipsField bettingField;
    [SerializeField]
    public BlackJackPlayerCardField blackJackField;
    void Start()
    {
        //bettingField = GetComponentInChildren<ChipsField>();
        //blackJackField = GetComponentInChildren<BlackJackPlayerCardField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
