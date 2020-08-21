using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [SerializeField] private RadialSector[] _sectors;

    public RadialSector[] Sectors { get => _sectors; }

    public RadialSector this[RadialSector.RadialMenuSector sector]
    {
        get => GetValue(sector);
    }

    private RadialSector GetValue(RadialSector.RadialMenuSector sector)
    {
        return Sectors.FirstOrDefault(sec => sec.radialMenuSector == sector);
    }
}
