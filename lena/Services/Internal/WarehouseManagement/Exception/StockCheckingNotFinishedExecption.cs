using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class StockCheckingNotFinishedExecption : InternalServiceException
  {
    public StockCheckingNotFinishedExecption(int stockCheckingId)
    {
      StockCheckingId = stockCheckingId;
    }

    public int StockCheckingId { get; }
  }
}
