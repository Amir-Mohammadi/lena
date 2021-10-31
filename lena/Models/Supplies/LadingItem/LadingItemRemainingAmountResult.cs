using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingItem
{
  public class LadingItemRemainingAmountResult
  {

    public int CargoItemId { get; set; }
    public double CargoItemQty { get; set; }
    public double TotalLadingAmount { get; set; }
    public byte UnitId { get; set; }
  }
}
