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
  public class NoRepairUnitIsDefinedForTheProductionLineException : InternalServiceException
  {
    public string ProductionLineName { get; set; }

    public NoRepairUnitIsDefinedForTheProductionLineException(string productionLineName)
    {
      this.ProductionLineName = productionLineName;
    }
  }
}
