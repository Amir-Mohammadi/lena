using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement.Exception
{
  public class ScrumProjectGroupNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ScrumProjectGroupNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
