using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{

  #region FiscalPeriod
  public class WarehouseFiscalPeriodNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public WarehouseFiscalPeriodNotFoundException(int id)
    {
      this.Id = id;
    }

  }

  public class WarehouseFiscalPeriodFromDateIsBiggerThanToDateException : InternalServiceException
  {
    public WarehouseFiscalPeriodFromDateIsBiggerThanToDateException()
    {
    }
  }
  #endregion
}
