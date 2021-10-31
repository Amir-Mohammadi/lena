using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Unit
{
  public class SumQtyItemInput
  {
    public double Qty;
    public double CanceledQty { get; set; }
    public byte UnitId { get; set; }
  }
}
