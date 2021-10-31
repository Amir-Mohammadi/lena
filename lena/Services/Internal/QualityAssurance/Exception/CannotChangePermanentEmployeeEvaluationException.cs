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
  public class CannotChangePermanentEmployeeEvaluationException : InternalServiceException
  {
    public int Id { get; set; }
    public CannotChangePermanentEmployeeEvaluationException(int id)
    {
      this.Id = id;
    }
  }
}
