using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class NotHaveSufficientAvailableQtyToConfirmGeneralStuffRequestException : InternalServiceException
  {
    public string StuffCode { get; set; }
    public double AvailableQty { get; set; }

    public NotHaveSufficientAvailableQtyToConfirmGeneralStuffRequestException(string stuffCode, double availableQty)
    {
      StuffCode = stuffCode;
      AvailableQty = availableQty;
    }
  }
}
