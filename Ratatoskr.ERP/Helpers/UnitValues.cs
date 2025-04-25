using Ratatoskr.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ratatoskr.App.Helpers;

public static class UnitValues
{
    public static List<Unit> All { get; } = Enum.GetValues(typeof(Unit)).Cast<Unit>().ToList();
}
