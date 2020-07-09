using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


abstract class MovementState
{
    protected OVRPlayerController _context;

    public MovementState(OVRPlayerController context)
    {
        _context = context;
    } 

    public abstract void Movement();
       
}

