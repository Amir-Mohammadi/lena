﻿using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class ProducedQtyCannotBiggerThanProductionOrderQty : InternalServiceException
  {
    public int ProductionOrderId { get; set; }
    public ProducedQtyCannotBiggerThanProductionOrderQty(int productionOrderId)
    {
      this.ProductionOrderId = productionOrderId;
    }
  }
}
