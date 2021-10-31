using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class WorkPlanStepNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public WorkPlanStepNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
