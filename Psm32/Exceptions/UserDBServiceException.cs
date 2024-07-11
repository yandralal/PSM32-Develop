using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Exceptions;

public class UserDBServiceException : Exception
{
    public UserDBServiceException(string message) : base(message)
    {
    }
}
