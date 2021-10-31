using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Unit
{
  public class SumQtyResult
  {
    public byte UnitId { get; set; }
    public double ConvertRatio { get; set; }
    public double Qty { get; set; }
    public double CanceledQty { get; set; }
    public string UnitName { get; set; }
  }
}
