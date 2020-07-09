using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


abstract class HandActionState
{
 
    protected OVRPlayerController _context;

    public HandActionState(OVRPlayerController context)
    {
        _context = context;
    }

    public abstract void Action();

       
}

