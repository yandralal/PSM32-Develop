using System;

namespace Psm32.Exceptions;

public class CommandMessageException :Exception
{
    public CommandMessageException(string message) : base(message)
    {
    }
}
