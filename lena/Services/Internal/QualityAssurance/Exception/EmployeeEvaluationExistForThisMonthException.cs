using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance.Exception
{
  public class EmployeeEvaluationExistForThisMonthException : InternalServiceException
  {
    public int EmployeeId { get; set; }
    public EmployeeEvaluationExistForThisMonthException(int employeeId)
    {
      this.EmployeeId = employeeId;
    }
  }
}
