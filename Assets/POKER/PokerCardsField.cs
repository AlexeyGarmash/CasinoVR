
using UnityEngine;

public class PokerCardsField : MonoBehaviour
{
    public PokerCardSlot[] playerCardSlots;

    public int currentAvailableSlot = 0;
    public int allSlotsCount;

    private void Start()
    {
        playerCardSlots = GetComponentsInChildren<PokerCardSlot>();
        allSlotsCount = playerCardSlots.Length;
    }


    public void ReceiveSomePokerCard(GameObject pokerCard)
    {
        if(playerCardSlots !=null && playerCardSlots.Length > 1)
        {
            if(currentAvailableSlot < allSlotsCount)
            {
                playerCardSlots[currentAvailableSlot].ReceiveCard(pokerCard);
                currentAvailableSlot++;
            }
        }
    }
}
