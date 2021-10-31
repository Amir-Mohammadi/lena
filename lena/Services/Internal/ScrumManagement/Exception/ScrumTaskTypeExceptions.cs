using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement.Exception
{
  public class ScrumTaskTypeNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ScrumTaskTypeNotFoundException(int id)
    {
      this.Id = id;
    }

  }
}
