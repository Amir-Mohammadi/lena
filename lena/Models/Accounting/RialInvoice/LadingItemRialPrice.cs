using lena.Domains.Enums;
namespace lena.Models.Accounting.RialInvoice
{
  public class LadingItemRialPrice
  {
    public int LadingId { get; set; }
    public int LadingItemId { get; set; }
    public double StuffRialPrice { get; set; }
    public double Qty { get; set; }
  }
}
