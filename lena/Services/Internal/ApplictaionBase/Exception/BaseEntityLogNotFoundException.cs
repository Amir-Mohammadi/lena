using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class BaseEntityLogNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public BaseEntityLogNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
