using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class ObjectExtantions
{
    public static bool IsNull(this object T)
    {
        return T == null;
    }

    public static bool IsNotNull(this object T)
    {
        return T != null;
    }
}

