using CommunityToolkit.Mvvm.Messaging.Messages;
using Ratatoskr.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratatoskr.App.Messages;

public class InvoiceSavedMessage : ValueChangedMessage<Invoice>
{
    public InvoiceSavedMessage(Invoice invoice) : base(invoice) { }
}
