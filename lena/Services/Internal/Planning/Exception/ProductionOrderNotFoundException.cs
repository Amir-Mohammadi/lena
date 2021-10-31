using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class ProductionOrderNotFoundException : InternalServiceException
  {
    public string Code { get; }
    public int Id { get; }

    public ProductionOrderNotFoundException(string code)
    {
      this.Code = code;
    }

    public ProductionOrderNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
