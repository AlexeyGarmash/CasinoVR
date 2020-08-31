using Assets.Scipts.BackJack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackJackFields : MonoBehaviour
{

    [SerializeField]
    public ChipsField bettingField;
    [SerializeField]
    public BlackJackPlayerCardField blackJackCardField;

    [SerializeField]
    public ChipsField bettingFieldForSplit;
    [SerializeField]
    public BlackJackPlayerCardField blackJackCardFieldForSplit;

   
    [SerializeField]
    public ChipsField SaveField;


}
