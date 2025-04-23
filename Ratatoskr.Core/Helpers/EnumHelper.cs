
using Ratatoskr.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratatoskr.Core.Helpers;

public class EnumHelpers
{
    public static IEnumerable<Unit> Units => Enum.GetValues(typeof(Unit)).Cast<Unit>();
    public static IEnumerable<TaxRate> TaxRates => Enum.GetValues(typeof(TaxRate)).Cast<TaxRate>();
    public static IEnumerable<InvoiceStatus> InvoiceStatuses => Enum.GetValues(typeof(InvoiceStatus)).Cast<InvoiceStatus>();
}
