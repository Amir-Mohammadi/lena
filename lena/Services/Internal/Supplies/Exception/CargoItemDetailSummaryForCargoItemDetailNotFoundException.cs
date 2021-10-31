using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class CargoItemDetailSummaryForCargoItemDetailNotFoundException : InternalServiceException
  {
    public int CargoItemDetailId { get; }

    public CargoItemDetailSummaryForCargoItemDetailNotFoundException(int cargoItemDetailId)
    {
      this.CargoItemDetailId = cargoItemDetailId;
    }
  }
}
