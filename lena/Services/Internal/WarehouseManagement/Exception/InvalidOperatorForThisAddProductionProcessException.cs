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
  class InvalidOperatorForThisAddProductionProcessException : InternalServiceException
  {
    public int EmployeeId { get; set; }
    public int ProductionOrderId { get; set; }

    public InvalidOperatorForThisAddProductionProcessException(int employeeId, int productionOrderId)
    {
      this.EmployeeId = EmployeeId;
      this.ProductionOrderId = productionOrderId;
    }
  }
}
