using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadialMenuV2 : MonoBehaviour
{
   
    [SerializeField] private RadialSectorV2[] sectors;

    public RadialSectorV2[] Sectors { get => sectors; }

    public RadialSectorV2 this[RadialMenuSectorV2 sectorType]
    {
        get => GetValue(sectorType);
    }


    private RadialSectorV2 GetValue(RadialMenuSectorV2 sectorType)
    {
        return Sectors.FirstOrDefault(sec => sec.RadialSectorType == sectorType);
    }

    public void ClearSectorsData()
    {
        sectors.ToList().ForEach(s => s.ClearActionInfo());
    }
    public void ShowSectors(RadialMenuSectors sectors, List<RadialActionInfo> actions)
    {
        switch (sectors)
        {
            case RadialMenuSectors.ONE:
                ShowOneSector(actions);
                break;
            case RadialMenuSectors.TWO:
                ShowTwoSectors(actions);
                break;
            case RadialMenuSectors.THREE:
                ShowThreeSectors(actions);
                break;
            case RadialMenuSectors.FOUR:
                ShowFourSectors(actions);
                break;
            case RadialMenuSectors.FIVE:
                ShowFiveSectors(actions);
                break;
            case RadialMenuSectors.SIX:
                ShowSixSectors(actions);
                break;
        }
    }

    private void ShowOneSector(List<RadialActionInfo> actions)
    {
        Sectors.ToList().ForEach(s => s.transform.localScale = Vector3.zero);

        SetSectorsData(new List<RadialMenuSectorV2>() { RadialMenuSectorV2.FIRST_1_SECTORS }, actions);
       
    }
    private void ShowTwoSectors(List<RadialActionInfo> actions)
    {
        Sectors.ToList().ForEach(s => s.transform.localScale = Vector3.zero);

        SetSectorsData(
            new List<RadialMenuSectorV2>()
            { 
                RadialMenuSectorV2.FIRST_2_SECTORS,
                RadialMenuSectorV2.SECOND_2_SECTORS },
            actions
        );
       
    }
    private void ShowThreeSectors(List<RadialActionInfo> actions)
    {
        Sectors.ToList().ForEach(s => s.transform.localScale = Vector3.zero);

        SetSectorsData(
           new List<RadialMenuSectorV2>()
           {
                RadialMenuSectorV2.FIRST_3_SECTORS,
                RadialMenuSectorV2.SECOND_3_SECTORS,
                RadialMenuSectorV2.THIRD_3_SECTORS
           },
           actions
       );
       
    }
    private void ShowFourSectors(List<RadialActionInfo> actions)
    {
        Sectors.ToList().ForEach(s => s.transform.localScale = Vector3.zero);

        SetSectorsData(
           new List<RadialMenuSectorV2>()
           {
                RadialMenuSectorV2.FIRST_4_SECTORS,
                RadialMenuSectorV2.SECOND_4_SECTORS,
                RadialMenuSectorV2.THIRD_4_SECTORS,
                RadialMenuSectorV2.FOURTH_4_SECTORS
           },
           actions
       );
    }
    private void ShowFiveSectors(List<RadialActionInfo> actions)
    {
        Sectors.ToList().ForEach(s => s.transform.localScale = Vector3.zero);

        SetSectorsData(
           new List<RadialMenuSectorV2>()
           {
                RadialMenuSectorV2.FIRST_5_SECTORS,
                RadialMenuSectorV2.SECOND_5_SECTORS,
                RadialMenuSectorV2.THIRD_5_SECTORS,
                RadialMenuSectorV2.FOURTH_5_SECTORS,
                RadialMenuSectorV2.FIFTH_5_SECTORS
           },
           actions
       );
    }
    private void ShowSixSectors(List<RadialActionInfo> actions)
    {
        Sectors.ToList().ForEach(s => s.transform.localScale = Vector3.zero);

        SetSectorsData(
           new List<RadialMenuSectorV2>()
           {
                RadialMenuSectorV2.FIRST_6_SECTORS,
                RadialMenuSectorV2.SECOND_6_SECTORS,
                RadialMenuSectorV2.THIRD_6_SECTORS,
                RadialMenuSectorV2.FOURTH_6_SECTORS,
                RadialMenuSectorV2.FIFTH_6_SECTORS,
                RadialMenuSectorV2.SIXTH_6_SECTORS
           },
           actions
       );
    }

    void SetSectorsData(List<RadialMenuSectorV2> sectors, List<RadialActionInfo> radialActionInfo)
    {
        Debug.Log("sectors" + sectors.Count);
        for (var i = 0; i < sectors.Count; i++)
        {
            var sector = Sectors.FirstOrDefault(s => s.RadialSectorType == sectors[i]);

            sector.transform.localScale = Vector3.one;
            
            sector.SetSectorData(radialActionInfo[i]);
        }
        
       
    }
}
