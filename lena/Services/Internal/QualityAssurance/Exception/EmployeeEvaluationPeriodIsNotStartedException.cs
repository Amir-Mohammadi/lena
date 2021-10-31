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
  public class EmployeeEvaluationPeriodIsNotStartedException : InternalServiceException
  {
    public int Id { get; set; }

    public EmployeeEvaluationPeriodIsNotStartedException(int id)
    {
      this.Id = id;
    }
  }
}
