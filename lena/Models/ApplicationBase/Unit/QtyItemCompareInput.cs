using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Unit
{
  public class QtyItemCompareInput
  {
    public CurrentQtyItemInput[] CurrentQtyItemInput { get; set; }
    public TargetQtyItemInput[] TargetQtyItemInput { get; set; }

  }
  public class CurrentQtyItemInput
  {
    public double CurrentQty;
    public byte CurrentUnitId { get; set; }
  }
  public class TargetQtyItemInput
  {
    public double TargetQty;
    public byte TargetUnitId { get; set; }
  }
}
