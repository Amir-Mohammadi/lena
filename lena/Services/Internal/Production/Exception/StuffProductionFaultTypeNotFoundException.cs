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
  public class StuffProductionFaultTypeNotFoundException : InternalServiceException
  {
    public int ProductionFaultTypeId { get; }
    public int StuffId { get; }

    public StuffProductionFaultTypeNotFoundException(int productionFaultTypeId, int stuffId)
    {
      ProductionFaultTypeId = productionFaultTypeId;
      StuffId = stuffId;
    }
  }
}
