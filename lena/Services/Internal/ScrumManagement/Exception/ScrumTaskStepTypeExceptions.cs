using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement.Exception
{
  public class ScrumTaskTypeStepNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ScrumTaskTypeStepNotFoundException(int id)
    {
      this.Id = id;
    }

  }
}
