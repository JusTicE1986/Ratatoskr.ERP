using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratatoskr.Core.Enums;

public enum InvoiceStatus
{
    Draft,
    Open,
    Paid,
    Overdue,
    Canceled,
    CreditNote
}
