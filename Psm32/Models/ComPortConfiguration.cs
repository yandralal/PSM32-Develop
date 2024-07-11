namespace Psm32.Models;

using System;
using System.Collections.Generic;
using System.IO.Ports;

public class ComPortConfiguration
{
    private static readonly string DEFAULT_COMPORT = "COM3";
    private static readonly int DEFAULT_BOUDRATE = 9600;
    private static readonly int DEFAULT_BYTESIZE = 8;
    private static readonly int DEFAULT_READ_TIMEOUT = 500;
    private static readonly int DEFAULT_WRITE_TIMEOUT = 500;
    public string PortName {get; set; }
    public int BoudRate { get; set; }
    public int ByteSize { get; set; }
    public Parity ParityValue { get; set; }
    public StopBits StopBitsValue { get; set; }
    public int ReadTimeOut { get; set; }
    public int WriteTimeOut { get; set; }

    public ComPortConfiguration(
        string? portName = null, 
        int? boudRate = null, 
        int? byteSize = null, 
        Parity? parity= null, 
        StopBits? stopBits = null, 
        int? readTimeOut = null, 
        int? writeTimeOut = null)
    {
        PortName = portName ?? DEFAULT_COMPORT;
        BoudRate = boudRate ?? DEFAULT_BOUDRATE;
        ByteSize = byteSize ?? DEFAULT_BYTESIZE;
        ParityValue = parity ?? Parity.None;
        StopBitsValue = stopBits ?? StopBits.One;
        ReadTimeOut = readTimeOut ?? DEFAULT_READ_TIMEOUT;
        WriteTimeOut = writeTimeOut ?? DEFAULT_WRITE_TIMEOUT;
    }

    public override bool Equals(object? obj)
    {
        ComPortConfiguration? config = obj as ComPortConfiguration;
        
        if (config == null)
        {
            return false;
        }

        return PortName == config.PortName
            && BoudRate == config.BoudRate
            && ByteSize == config.ByteSize
            && ParityValue == config.ParityValue
            && StopBitsValue == config.StopBitsValue
            && ReadTimeOut == config.ReadTimeOut
            && WriteTimeOut == config.WriteTimeOut;
    }

    public override int GetHashCode()
    {
        return PortName.GetHashCode()
            ^ BoudRate.GetHashCode()
            ^ ByteSize.GetHashCode()
            ^ ParityValue.GetHashCode()
            ^ StopBitsValue.GetHashCode()
            ^ ReadTimeOut.GetHashCode()
            ^ WriteTimeOut.GetHashCode();
    }

    public Dictionary<string, string> ToDTODictionary()
    {
        return new Dictionary<string, string>
        {
            {"PortName", PortName },
            {"BoudRate", BoudRate.ToString()},
            {"ReadTimeOut", ReadTimeOut.ToString()},
            {"WriteTimeOut", WriteTimeOut.ToString()},
        };
    }

    public static ComPortConfiguration FromDTODictionary(Dictionary<string, string> config)
    {
        return new ComPortConfiguration()
        {
            PortName = config.ContainsKey("PortName") ? config["PortName"] : DEFAULT_COMPORT,
            BoudRate = config.ContainsKey("BoudRate") ? Int32.Parse(config["BoudRate"]) : DEFAULT_BOUDRATE,
            ReadTimeOut = config.ContainsKey("ReadTimeOut") ? Int32.Parse(config["ReadTimeOut"]) : DEFAULT_READ_TIMEOUT,
            WriteTimeOut = config.ContainsKey("WriteTimeOut") ? Int32.Parse(config["WriteTimeOut"]) : DEFAULT_WRITE_TIMEOUT,
        };
    }
}
