using Ratatoskr.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratatoskr.Core.Helpers;

public static class UnitExtensions
{
    public static string ToDisplay(this Unit unit) => unit switch
    {
        Unit.Stück => "Stück",
        Unit.Stunde => "Stunde(n)",
        Unit.Tag => "Tag(e)",
        Unit.Pauschale => "Pauschale",
        Unit.Liter => "Liter",
        Unit.Kilogramm => "Kilogramm",
        _ => unit.ToString()
    };
}
