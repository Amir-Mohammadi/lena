using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class ProductionNotFoundException : InternalServiceException
  {
    public string Code { get; }
    public int Id { get; }

    public ProductionNotFoundException(string code)
    {
      this.Code = code;
    }

    public ProductionNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
