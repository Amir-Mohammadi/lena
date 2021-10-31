using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class DuplicateProductionOperatorException : InternalServiceException
  {
    public int ProductionOperatorId { get; }
    public string ProductionCode { get; set; }
    public string OperationCode { get; set; }

    public DuplicateProductionOperatorException(
        int productionOperatorId,
        string productionCode = null,
        string operationCode = null
        )
    {
      this.ProductionOperatorId = productionOperatorId;
      this.ProductionCode = productionCode;
      this.OperationCode = operationCode;
    }
  }
}
