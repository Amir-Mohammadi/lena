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
  public class StuffCannotBeConsumedByThisProductionOrderException : InternalServiceException
  {
    public int StuffId { get; set; }
    public int ProductionOrderId { get; set; }

    public StuffCannotBeConsumedByThisProductionOrderException(int stuffId, int productionOrderId)
    {
      StuffId = stuffId;
      ProductionOrderId = productionOrderId;
    }
  }
}
