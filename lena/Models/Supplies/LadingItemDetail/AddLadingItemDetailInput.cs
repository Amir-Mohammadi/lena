using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingItemDetail
{
  public class AddLadingItemDetailInput
  {
    public int CargoItemId { get; set; }
    public int CargoItemDetailId { get; set; }
    public double Qty { get; set; }
    public int? LadingItemId { get; set; }

  }
  public class SumQtyLadingItemDetailInput
  {
    public int CargoItemId { get; set; }
    public double LadingItemQty { get; set; }
    public IQueryable<CargoItemDetails> CargoItemDetails { get; set; }
  }
  public class CargoItemDetails
  {
    public int CargoItemDetailId { get; set; }
    public double LadingItemDetailQty { get; set; }
  }

  public class CargoItemInfoResult
  {
    public int CargoItemId { get; set; }
    public int ProviderId { get; set; }
  }

}
