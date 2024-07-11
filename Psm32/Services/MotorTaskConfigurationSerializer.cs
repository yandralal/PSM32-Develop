using Psm32.Models;
using System.Text.Json;

namespace Psm32.Services;

public class MotorTaskConfigurationSerializer
{
    public string ToJson(MotorTaskConfiguration motorTaskConfiguration)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Serialize(motorTaskConfiguration, options);
    }

    public MotorTaskConfiguration? FromJson(string motorTaskConfiguration)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        return JsonSerializer.Deserialize<MotorTaskConfiguration>(motorTaskConfiguration, options);
    }
}
