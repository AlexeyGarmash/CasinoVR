using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BetStackData : StackData
{
    public int bet;

    public override void ClearData()
    {
        base.ClearData();
        bet = 0;
    }
}
