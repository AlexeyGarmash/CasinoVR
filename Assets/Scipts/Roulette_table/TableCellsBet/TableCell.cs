using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TableCell : MonoBehaviourPun
{
    [SerializeField] public int WinKoeff;
    public List<BetData> BetsData { get; private set; }

    [SerializeField] private List<BetData> _showBetsData;
   
    private void Awake()
    {
        BetsData = new List<BetData>();
        _showBetsData = BetsData;
    }

    public void ReceiveBetData(BetData betData)
    {
        var findDataIndex = FindBetDataIndex(betData);
        if (findDataIndex != -1)
        {
            UpdateExistBet(findDataIndex, betData);
        }
        else
        {
            AddNewBet(betData);
        }
    }

    private void AddNewBet(BetData betData)
    {
        //BetsData.Add(betData);
        photonView?.RPC("AddNewBet_RPC", RpcTarget.All, betData.ToByteArray());
        print(string.Format("New Player {0} add bet {1}", betData.PlayerStat.PlayerNick, betData.BetValue));
    }

    private void UpdateExistBet(int index, BetData betData)
    {
        //BetsData[index].AddBetValue(betData.BetValue);
        photonView?.RPC("UpdateExistBet_RPC", RpcTarget.All, index, betData.ToByteArray());
        print(string.Format("Exiested Player {0} add bet {1}", betData.PlayerStat.PlayerNick, betData.BetValue));
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
                RemoveExistBetValues(findDataIndex, betData);
                print(string.Format("Player {0} remove bet {1}", betData.PlayerStat.PlayerNick, betData.BetValue));
            }
        }
    }

    private void RemoveExistBetValues(int index, BetData betData)
    {
        photonView?.RPC("RemoveBetValues_RPC", RpcTarget.Others, index, betData.ToByteArray());
    }

    public void ResetBets(BetData betData)
    {
        //BetsData.RemoveAll(data => data.PlayerStat.PlayerNick.Equals(betData.PlayerStat.PlayerNick));
        photonView?.RPC("ResetBets_RPC", RpcTarget.All, betData.ToByteArray());
    }
    private int FindBetDataIndex(BetData betData) => BetsData.FindIndex(data => data.PlayerStat.PlayerNick.Equals(betData.PlayerStat.PlayerNick));

    
    //}

    public abstract bool CheckIsWinCell(WheelCellData wheelCellData);


    #region RPCs

    [PunRPC]
    public void AddNewBet_RPC(byte[] betData)
    {
        BetsData.Add(betData.FromByteArray() as BetData);
    }

    [PunRPC]
    public void UpdateExistBet_RPC(int index, byte[] betData)
    {
        BetsData[index].AddBetValue((betData.FromByteArray() as BetData).BetValue);
    }

    [PunRPC]
    public void ResetBets_RPC(byte[] betData)
    {
        BetsData.RemoveAll(data => data.PlayerStat.PlayerNick.Equals((betData.FromByteArray() as BetData).PlayerStat.PlayerNick));
    }

    [PunRPC]
    public void RemoveBetValues_RPC(int index, byte[] betData)
    {
        BetsData[index].RemoveBetValue((betData.FromByteArray() as BetData).BetValue);
    }
    #endregion
}
