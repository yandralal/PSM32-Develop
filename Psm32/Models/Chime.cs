using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Models;

public class Chime: IEquatable<Chime>
{
    TimeOnly CueTime { get; set; }

    public bool Equals(Chime? chime)
    {
        if (chime == null)
        {
            return false;
        }

        return CueTime.Equals(chime.CueTime);
    }

    public override bool Equals(object? obj) => Equals(obj as Chime);

    public override int GetHashCode()
    {
        return HashCode.Combine(CueTime);
    }
}
