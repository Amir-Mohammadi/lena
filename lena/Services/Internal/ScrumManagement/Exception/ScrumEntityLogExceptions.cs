using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement.Exception
{
  public class ScrumEntityLogNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ScrumEntityLogNotFoundException(int id)
    {
      this.Id = id;
    }

  }
}
