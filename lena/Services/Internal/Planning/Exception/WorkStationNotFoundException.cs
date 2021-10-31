using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{

  public class WorkStationNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public WorkStationNotFoundException(int id)
    {
      this.Id = id;
    }

  }
}
