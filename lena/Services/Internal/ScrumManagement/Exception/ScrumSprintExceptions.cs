using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement.Exception
{
  public class ScrumSprintNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ScrumSprintNotFoundException(int id)
    {
      this.Id = id;
    }

  }
}
