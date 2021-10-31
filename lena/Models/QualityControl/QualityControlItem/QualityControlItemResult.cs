using lena.Models.QualityControl.QualityControlSample;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlItem
{
  public class QualityControlItemResult
  {
    public int Id { get; set; }
    public int QualityControlId { get; set; }
    public string Code { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public long? StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public bool Status { get; set; }
    public double Qty { get; set; }
    public double? ConsumeQty { get; set; }
    public double? RemainedQty { get; set; }
    public double? TestQty { get; set; }

    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public string Description { get; set; }
    public IQueryable<QualityControlSampleResult> QualityControlSamples { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
