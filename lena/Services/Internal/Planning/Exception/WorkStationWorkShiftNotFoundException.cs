using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class ProductionLineWorkShiftNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProductionLineWorkShiftNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
