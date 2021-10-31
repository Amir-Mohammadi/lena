using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class ProductionOperationNotFoundException : InternalServiceException
  {
    public string Code { get; }
    public int Id { get; }

    public ProductionOperationNotFoundException(string code)
    {
      this.Code = code;
    }

    public ProductionOperationNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
