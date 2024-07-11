using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Core;

public class Contraction
{
    Contraction()
    {
        StartTime = 0;
        RampUp = 0;
        RampDn = 0;
        RunTime = 0;
    }

    public int StartTime { get; }
    public int RampUp { get; }
    public int RampDn { get; }
    public int RunTime { get;}
}
