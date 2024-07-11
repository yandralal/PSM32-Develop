using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Exceptions;

public class QSUException:Exception
{
    public QSUException(string message) : base(message)
    {
    }
}
