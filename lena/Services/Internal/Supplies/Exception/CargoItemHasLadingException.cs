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
  public class CargoItemHasLadingException : InternalServiceException
  {
    public int CargoItemId { get; set; }

    public CargoItemHasLadingException(int cargoItemId)
    {
      this.CargoItemId = cargoItemId;
    }
  }
}
