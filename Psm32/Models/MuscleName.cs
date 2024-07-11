using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Models;

public class MuscleName
{
    public MuscleName()
    {
    }

    public MuscleName(string? name = null, string? shortName = null)
    {
        if (shortName == null && name == null)
        {
            return;
        }

        if (name != null)
        {
            Name = name;
        }

        if (shortName != null)
        {
            ShortName = shortName;
        }

        if (name == null)
        {
           // TODO: get name by short name
        }

        if (shortName == null)
        {
            //TODO: get short name by name
        }
    }

    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
}
