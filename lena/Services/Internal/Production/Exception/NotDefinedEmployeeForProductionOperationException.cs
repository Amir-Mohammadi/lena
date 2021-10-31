using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class NotDefinedEmployeeForProductionOperationException : InternalServiceException
  {
    public int? ProductionOrderId { get; set; }
    public string Serial { get; set; }
    public int? ProductionOperatorId { get; }
    public int OperationId { get; }
    public string OperationTitle { get; set; }
    public NotDefinedEmployeeForProductionOperationException(int? productionOperatorId, int operationId, string operationTitle, int? productionOrderId = null, string serial = null)
    {
      this.ProductionOperatorId = productionOperatorId;
      this.OperationId = operationId;
      this.ProductionOrderId = productionOrderId;
      this.Serial = serial;
      this.OperationTitle = operationTitle;
    }
  }
}
