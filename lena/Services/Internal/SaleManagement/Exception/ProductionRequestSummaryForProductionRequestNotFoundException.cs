using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class ProductionRequestSummaryForProductionRequestNotFoundException : InternalServiceException
  {
    public int ProductionRequestId { get; }

    public ProductionRequestSummaryForProductionRequestNotFoundException(int productionRequestId)
    {
      this.ProductionRequestId = productionRequestId;
    }
  }
}
