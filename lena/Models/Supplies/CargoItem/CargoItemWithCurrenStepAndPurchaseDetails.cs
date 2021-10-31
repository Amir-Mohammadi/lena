using lena.Models.Supplies.Ladings;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class CargoItemWithCurrenStepAndPurchaseDetails
  {
    public lena.Domains.CargoItem CargoItem { get; set; }

    public lena.Domains.PurchaseStep PurchaseStep { get; set; }
    public lena.Domains.PurchaseOrder PurchaseOrder { get; set; }
    IQueryable<LadingQueryResult> Ladings { get; set; }
  }
}
