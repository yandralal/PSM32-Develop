using Psm32.Models;
using System;

namespace Psm32.Exceptions;

public class MotorTaskException : Exception
{
    public MotorTaskException(string message) : base(message)
    {
    }
}
