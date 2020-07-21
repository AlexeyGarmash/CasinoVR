using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RowTableCell : TableCell
{
    [SerializeField, Tooltip("Start from bottom")] private int RowNum;

    private int[][] Nums = new int[][]
    {
        GenerateNumRow(1),
        GenerateNumRow(2),
        GenerateNumRow(3),
    };

    private static int[] GenerateNumRow(int from, int count = 12, int step = 3)
    {
        int[] retRow = new int[count];
        for (int i = 1; i <= count; i++)
        {
            retRow[i - 1] = i * step;
        }
        return retRow;
    }
    public override bool CheckIsWinCell(WheelCellData wheelCellData) => Nums[RowNum - 1].Contains(wheelCellData.Number);

}
