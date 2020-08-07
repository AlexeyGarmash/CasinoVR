using BansheeGz.BGSpline.Curve;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinAnimation : MonoBehaviourPun
{
    
    [SerializeField]
    BezierCurve[] curves;

    [SerializeField]
    float speed = 0.1f;
    [SerializeField]
    float MinSpeed = 1;

    [SerializeField]
    float speedSlowingStep = 0.5f;

    private void Start()
    {
        winChips = new List<GameObject>();
        curves = GetComponentsInChildren<BezierCurve>();

       



    }
    List<GameObject> winChips;
  
    [PunRPC]
    public void StartAnimation_RPC()
    {
        StartCoroutine(MoveChipWithCurve());
    }
    public void StartAnimation(int win, string nickName)
    {
        CreateWinChips(win, nickName);
        photonView.RPC("StartAnimation_RPC", RpcTarget.All);    

    }
    private void CreateWinChips(int money, string nickName)
    {
        
        var starmoney = money;
        Chips chipCost;
        
        while (money > 0)
        {
           
            chipCost = Chips.YELLOW;
            if (starmoney / 2 < money && money > (int)Chips.PURPLE)           
                chipCost = Chips.PURPLE;
            
            else if (starmoney / 4 < money && money > (int)Chips.BLACK)            
                chipCost = Chips.BLACK;

            else if (starmoney / 8 < money && money > (int)Chips.GREEN)           
                chipCost = Chips.GREEN;
    
            else if (starmoney / 16 < money && money > (int)Chips.BLUE)           
                chipCost = Chips.BLUE;              
            
            else if (starmoney / 32 < money && money > (int)Chips.RED)           
                chipCost = Chips.RED;               
                


            var chip = PhotonNetwork.Instantiate(ChipUtils.Instance.GetPathToChip(chipCost), transform.position, transform.rotation);
            chip.GetComponent<ItemNetworkInfo>().Owner = nickName;
            money -= (int)chipCost;
        }

       
    }
    IEnumerator MoveChipWithCurve()
    {

        yield return new WaitForSeconds(1f);

        Debug.Log(winChips);

        while (winChips.Count != 0)
        { 
            var curvePurpel = curves[Random.Range(0, curves.Length - 1)];
            var chip = winChips[Random.Range(0, winChips.Count - 1)];

            winChips.Remove(chip);
            
            StartCoroutine(MoveOneChip(curvePurpel, chip));
            yield return new WaitForSeconds(0.1f);
            
        }

        yield return null;

    }

    IEnumerator MoveOneChip(BezierCurve curvePurpel, GameObject chip)
    {
        var localSpeed = speed;
        float t = 0;
        t += Time.deltaTime * speed;
        chip.SetActive(true);

        chip.GetComponent<Collider>().enabled = false;
        chip.GetComponent<Rigidbody>().isKinematic = true;
        while (t != 1)
        {
            //localSpeed -= speedSlowingStep;
            if (t > 0.95)
                t = 1;
            chip.transform.position = curvePurpel.GetPointAt(t);
            chip.transform.Rotate(Random.Range(10f, 30f), Random.Range(10f, 30f), Random.Range(10f, 30f));
            yield return null;
            t += Time.deltaTime * localSpeed;
            if (t > 0.95)
                t = 1;
        }

        chip.GetComponent<Collider>().enabled = true;
        chip.GetComponent<Rigidbody>().isKinematic = false;

        yield return null;
        
    }

    private void OnTriggerEnter(Collider other)
    {
       
        var chip = other.gameObject.GetComponent<ChipData>();           
        var view = other.gameObject.GetComponent<PhotonView>();
       
        if (chip != null && view != null)
        {
            winChips.Add(other.gameObject);
           
            other.gameObject.SetActive(false);
            view.GetComponent<PhysicsSmoothView>().SyncOff();

        }

    }


}

