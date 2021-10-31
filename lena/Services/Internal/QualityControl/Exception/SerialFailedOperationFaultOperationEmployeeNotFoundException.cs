using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class SerialFailedOperationFaultOperationEmployeeNotFoundException : InternalServiceException
  {
    public int ProductionOperationEmployeeId { get; set; }
    public int SerialFailedOperationFaultOperationId { get; set; }
    public SerialFailedOperationFaultOperationEmployeeNotFoundException(int productionOperationEmployeeId, int serialFailedOperationFaultOperationId)
    {
      this.ProductionOperationEmployeeId = productionOperationEmployeeId;
      this.SerialFailedOperationFaultOperationId = serialFailedOperationFaultOperationId;
    }
  }
}
