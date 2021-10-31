using lena.Services.Core.Exceptions;
using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class RiskHaveNotActionResolveException : InternalServiceException
  {
    public int RiskId { get; set; }

    public RiskHaveNotActionResolveException(int riskId)
    {
      this.RiskId = riskId;
    }
  }
}
