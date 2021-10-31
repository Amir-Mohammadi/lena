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
  public class DuplicateProductionOperatorMachineEmployeeException : InternalServiceException
  {
    public int? EmployeeId { get; }
    public int ProductionOperatorId { get; }
    public DuplicateProductionOperatorMachineEmployeeException(int? employeeId, int productionOperatorId)
    {
      this.EmployeeId = employeeId;
      this.ProductionOperatorId = productionOperatorId;
    }
  }
}
