using System.ComponentModel.DataAnnotations;

namespace Psm32.DB;

public class ConfigurationItemDTO
{
    public ConfigurationItemDTO(string key, string value)
    {
        Key = key;
        Value = value;
    }

    [Key]
    public string Key { get; set; }
    public string Value { get; set; }
}
