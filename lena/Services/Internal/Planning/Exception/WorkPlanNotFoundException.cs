using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class WorkPlanNotFoundException : InternalServiceException
  {
    public WorkPlanNotFoundException(int id)
    {
      Id = id;
    }

    public int Id { get; }
  }
}
