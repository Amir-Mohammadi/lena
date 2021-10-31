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
  public class RiskHaveEffectiveRiskResolveException : InternalServiceException
  {
    public int RiskId { get; set; }
    public RiskHaveEffectiveRiskResolveException(int riskId)
    {
      this.RiskId = riskId;
    }
  }
}
