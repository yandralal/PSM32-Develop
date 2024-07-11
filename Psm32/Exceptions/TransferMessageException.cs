using System;

namespace Psm32.Exceptions;

public class TransferMessageException :Exception
{
    public TransferMessageException(string message) : base(message)
    {
    }
}
