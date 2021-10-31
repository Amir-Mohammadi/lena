
using lena.Domains.Enums;
namespace lena.Models.Supplies.Cargo
{
  public class CargoItemWithCurrentStep
  {
    public Domains.CargoItem CargoItem { get; set; }
    public Domains.PurchaseStep PurchaseStep { get; set; }
  }
}