using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class NoMatchCurrencyRateFoundException : InternalServiceException
  {
    public int FromCurrencyId { get; set; }
    public int ToCurrencyId { get; set; }

    public NoMatchCurrencyRateFoundException(int fromCurrencyId, int toCurrencyId)
    {
      FromCurrencyId = fromCurrencyId;
      ToCurrencyId = toCurrencyId;
    }
  }
}
