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
  public class OpenPackageIsExistException : InternalServiceException
  {
    public int ProductionOperatorId { get; }
    public int? ProductionId { get; }
    public OpenPackageIsExistException(int productionOperatorId, int? productionId)
    {
      this.ProductionOperatorId = productionOperatorId;
      this.ProductionId = productionId;
    }
  }
}