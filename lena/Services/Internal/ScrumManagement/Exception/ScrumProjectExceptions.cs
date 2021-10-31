using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement.Exception
{
  public class ScrumProjectNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ScrumProjectNotFoundException(int id)
    {
      this.Id = id;
    }

  }
}
