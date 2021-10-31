using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class WorkStationOperationNotFoundException : InternalServiceException
  {
    public int OperationId { get; }
    public int WorkStationId { get; }

    public WorkStationOperationNotFoundException(int workStationId, int operationId)
    {
      this.WorkStationId = workStationId;
      this.OperationId = operationId;
    }
  }
}
