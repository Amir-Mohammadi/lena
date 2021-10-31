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
  public class CurrencyRateNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public CurrencyRateNotFoundException(int id)
    {
      Id = id;
    }
  }
}
