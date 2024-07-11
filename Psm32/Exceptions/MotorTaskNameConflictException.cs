using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Exceptions;

public class MotorNameConflictTaskException   : Exception
{
    public MotorNameConflictTaskException(string message) : base(message)
    {
    }
}
