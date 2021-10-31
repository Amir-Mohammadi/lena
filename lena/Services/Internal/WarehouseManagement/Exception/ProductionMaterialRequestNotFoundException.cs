using lena.Services.Core.Foundation;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class ProductionMaterialRequestNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public string Code { get; }
    public ProductionMaterialRequestNotFoundException(int id)
    {
      this.Id = id;
    }
    public ProductionMaterialRequestNotFoundException(string code)
    {
      this.Code = code;
    }
  }
}
