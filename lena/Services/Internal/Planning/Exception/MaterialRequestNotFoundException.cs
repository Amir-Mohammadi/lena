using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class MaterialRequestNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public MaterialRequestNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
