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
  public class LadingItemQtyCannotBeMoreThanCargoAvaliableAmountException : InternalServiceException
  {
    public int CargoItemId { get; set; }
    public double RequesteAmount { get; set; }
    public double AvaliableAmount { get; set; }

    public LadingItemQtyCannotBeMoreThanCargoAvaliableAmountException(int cargoItemId, double requestedAmount, double avilableAmount)
    {
      AvaliableAmount = avilableAmount;
      RequesteAmount = requestedAmount;
      CargoItemId = cargoItemId;
    }
  }
}
