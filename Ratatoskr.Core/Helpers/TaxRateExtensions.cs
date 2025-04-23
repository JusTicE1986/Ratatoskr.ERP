using Ratatoskr.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ratatoskr.Core.Helpers;

public static class TaxRateExtensions
{
    public static decimal ToDecimal(this TaxRate rate) => (int)rate / 100m;

	public static string ToDisplay(this TaxRate rate) => rate switch
	{
		TaxRate.Zero => "0 % (steuerfrei)",
		TaxRate.Reduced => "7 % (ermäßigt)",
		TaxRate.Regular => "19 % (regulär)",
		_ => $"{(int)rate} %"
	};
}
