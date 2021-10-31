using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement.Exception
{
  public class ScrumTaskStepNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ScrumTaskStepNotFoundException(int id)
    {
      this.Id = id;
    }

  }
}
