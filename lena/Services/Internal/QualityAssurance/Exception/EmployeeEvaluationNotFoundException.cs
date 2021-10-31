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
  public class EmployeeEvaluationNotFoundException : InternalServiceException
  {
    public int? Id { get; set; }

    public int? EmployeeId { get; set; }

    public EmployeeEvaluationNotFoundException(int? id = null, int? employeeId = null)
    {
      this.EmployeeId = employeeId;
      this.Id = id;
    }
  }
}
