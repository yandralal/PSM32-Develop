using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psm32.Models;

public class Rest: IEquatable<Rest>
{
    public TimeOnly StartTime { get; set; }
    public TimeSpan Duration { get; set; }

    public bool Equals(Rest? rest)
    {
        if (rest == null)
        {
            return false;
        }

        return StartTime.Equals(rest.StartTime) &&
               TimeSpan.Compare(Duration, rest.Duration) == 0;
    }

    public override bool Equals(object? obj) => Equals(obj as Rest);

    public override int GetHashCode()
    {
        return HashCode.Combine(StartTime, Duration);
    }
}
