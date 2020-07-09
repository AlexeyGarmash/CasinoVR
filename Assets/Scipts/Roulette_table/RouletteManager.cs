using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ROULETTE_EVENT { PLAYER_BET, WIN_NUMBER, PLAYER_CANCEL_BET  }
public class RouletteManager : MonoBehaviour
{
    EventManager<ROULETTE_EVENT> eventManager = new EventManager<ROULETTE_EVENT>();
}
