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
}
