using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class CustomsDeclarationNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public CustomsDeclarationNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
