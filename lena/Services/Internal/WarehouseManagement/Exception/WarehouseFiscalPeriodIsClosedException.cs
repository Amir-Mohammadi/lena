using lena.Services.Core.Foundation;
using System;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class WarehouseFiscalPeriodIsClosedException : InternalServiceException
  {
    public short Id { get; set; }

    public WarehouseFiscalPeriodIsClosedException(short id)
    {
      Id = id;
    }
  }
}
