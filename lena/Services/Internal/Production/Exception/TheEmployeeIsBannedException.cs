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
  public class TheEmployeeIsBannedException : InternalServiceException
  {
    public string ProductionOrderCode { get; set; }
    public string OperationTitle { get; set; }
    public string EmployeeFullName { get; set; }

    public TheEmployeeIsBannedException(string productionOrderCode, string operationTitle, string employeeFullName)
    {
      this.ProductionOrderCode = productionOrderCode;
      this.OperationTitle = operationTitle;
      this.EmployeeFullName = employeeFullName;
    }
  }
}
