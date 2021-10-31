using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingItemDetail
{
  public class LadingItemDetailRemainingAmountResult
  {

    public int CargoItemDetailId { get; set; }
    public double CargoItemDetailQty { get; set; }
    public double TotalLadingItemDetailAmount { get; set; }
    public byte UnitId { get; set; }
  }
}
