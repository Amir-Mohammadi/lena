using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Unit
{
  public class SumQtyInput
  {
    public byte? TargetUnitId { get; set; }
    public SumQtyItemInput[] SumQtyItems { get; set; }
  }
}
