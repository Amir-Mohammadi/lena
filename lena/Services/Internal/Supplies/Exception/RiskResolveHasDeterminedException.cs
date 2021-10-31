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
  public class RiskResolveHasDeterminedException : InternalServiceException
  {
    public int RiskResolveId { get; set; }

    public RiskResolveHasDeterminedException(int riskResolveId)
    {
      this.RiskResolveId = riskResolveId;
    }
  }
}
