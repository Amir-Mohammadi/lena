using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class IsDefaultSuppliesPurchaserUserException : InternalServiceException
  {
    public int StuffId { get; }
    public IsDefaultSuppliesPurchaserUserException(int stuffId)
    {
      StuffId = stuffId;
    }
  }

}
