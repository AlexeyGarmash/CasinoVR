using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TableCell : MonoBehaviour
{
    [SerializeField] public int WinKoeff;
    public List<BetData> BetsData { get; private set; }

   
    private void Awake()
    {
        
        BetsData = new List<BetData>();

       
    }

    public void ReceiveBetData(BetData betData)
    {
        var findDataIndex = FindBetDataIndex(betData);
        if (findDataIndex != -1)
        {
            BetsData[findDataIndex].AddBetValue(betData.BetValue);
            print(string.Format("Exiested Player {0} add bet {1}", betData.PlayerStat.PlayerNick, betData.BetValue));
        }
        else
        {
            BetsData.Add(betData);
            print(string.Format("New Player {0} add bet {1}", betData.PlayerStat.PlayerNick, betData.BetValue));
        }
    }

    public void RemoveBetData(BetData betData)
    {
        var findDataIndex = FindBetDataIndex(betData);
        if(findDataIndex != -1)
        {
            if(!BetsData[findDataIndex].RemoveBetValue(betData.BetValue))
            {
                ResetBets(betData);
                print(string.Format("Player {0} reset all bets", betData.PlayerStat.PlayerNick));
            } else
            {
                print(string.Format("Player {0} remove bet {1}", betData.PlayerStat.PlayerNick, betData.BetValue));
            }
        }
    }

    public void ResetBets(BetData betData)
    {
        BetsData.RemoveAll(data => data.PlayerStat.PlayerNick.Equals(betData.PlayerStat.PlayerNick));
    }
    private int FindBetDataIndex(BetData betData) => BetsData.FindIndex(data => data.PlayerStat.PlayerNick.Equals(betData.PlayerStat.PlayerNick));

    
    //}

    public abstract bool CheckIsWinCell(WheelCellData wheelCellData);
   
}
