using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class ProducstionYearHasBeenUsedInMakingIranKhodroSerial : InternalServiceException
  {
    public int ProductionYearId { get; }
    public string Code { get; }

    public ProducstionYearHasBeenUsedInMakingIranKhodroSerial(int productiojYearId, string code)
    {
      this.ProductionYearId = productiojYearId;
      this.Code = code;
    }

  }
}
