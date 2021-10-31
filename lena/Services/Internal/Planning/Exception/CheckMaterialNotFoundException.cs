using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class CheckMaterialNotFoundException : InternalServiceException
  {

    public int Id { get; }

    public CheckMaterialNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
