using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class StuffProductionFaultTypeExistException : InternalServiceException
  {
    public int StuffId { get; }
    public int ProductionFaultTypeId { get; }
    public StuffProductionFaultTypeExistException(int stuffId, int productionFaultTypeId)
    {
      StuffId = stuffId;
      ProductionFaultTypeId = productionFaultTypeId;

    }
  }
}
