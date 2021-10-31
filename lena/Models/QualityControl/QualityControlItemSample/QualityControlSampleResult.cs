using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlSample
{
  public class QualityControlSampleResult
  {
    public int? Id { get; set; }
    public string Code { get; set; }
    public double? Qty { get; set; }
    public QualityControlSampleStatus? Status { get; set; }
    public int QualityControlItemId { get; set; }
    public int QualityControlId { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public long? StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public bool QualityControlItemStatus { get; set; }
    public double QualityControlItemQty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public string QualityControlItemDescription { get; set; }

    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public int? StatusChangerUserId { get; set; }
    public string StatusChangerEmployeeFullName { get; set; }
    public DateTime DateTime { get; set; }
    public double? TestQty { get; set; }
    public double? ConsumeQty { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
