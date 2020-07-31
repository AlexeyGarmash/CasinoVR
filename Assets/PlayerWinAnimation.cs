using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinAnimation : MonoBehaviour
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
        curves = GetComponentsInChildren<BezierCurve>();
        
        
    }
    List<GameObject> winChips;

    public void StartAnimation(int money)
    {
        CreateWinChips(money);
        StartCoroutine(MoveChipWithCurve());
    }
    private void CreateWinChips(int money)
    {
        winChips = new List<GameObject>();
        var starmoney = money;
        Chips chipCost;
        
        while (money > 0)
        {
            GameObject chip;
            chipCost = Chips.YELLOW;
            if (starmoney / 2 < money && money > (int)Chips.PURPLE)
            {
                chipCost = Chips.PURPLE;
                chip = Instantiate(ChipUtils.Instance.purpleChipPrefab, transform);
                

            }
            else if (starmoney / 4 < money && money > (int)Chips.BLACK)
            {
                chipCost = Chips.BLACK;
                chip = Instantiate(ChipUtils.Instance.blackChipPrefab, transform);
                
              

            }
            else if (starmoney / 8 < money && money > (int)Chips.GREEN)
            {
                chipCost = Chips.GREEN;
                chip = Instantiate(ChipUtils.Instance.greenChipPrefab, transform);


            }
            else if (starmoney / 16 < money && money > (int)Chips.BLUE)
            {
                chipCost = Chips.BLUE;
                
                chip = Instantiate(ChipUtils.Instance.blueChipPrefab, transform);

            }
            else if (starmoney / 32 < money && money > (int)Chips.RED)
            {
                chipCost = Chips.RED;               
                chip = Instantiate(ChipUtils.Instance.redChipPrefab, transform);

            }
            else
                chip = Instantiate(ChipUtils.Instance.yellowChipPrefab, transform);

            chip.SetActive(false);
            winChips.Add(chip);
            money -= (int)chipCost;
        }
    }
    IEnumerator MoveChipWithCurve()
    {
        
        while(winChips.Count != 0)
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


}

