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
  public class CargoItemSummaryNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public CargoItemSummaryNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
