using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class ProductionTerminalNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProductionTerminalNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
