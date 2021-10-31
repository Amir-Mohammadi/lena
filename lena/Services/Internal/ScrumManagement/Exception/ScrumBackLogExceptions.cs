using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement.Exception
{
  public class ScrumBackLogNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ScrumBackLogNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
