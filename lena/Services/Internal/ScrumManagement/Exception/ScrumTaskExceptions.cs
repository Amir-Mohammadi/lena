using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement.Exception
{
  public class ScrumTaskNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ScrumTaskNotFoundException(int id)
    {
      this.Id = id;
    }

  }
}
