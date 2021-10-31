using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlSample
{
  public class QualityControlSampleFullResult
  {
    public int QualityControlId { get; set; }
    public string QualityControlCode { get; set; }
    public double QualityControlQty { get; set; }
    public QualityControlStatus QualityControlStatus { get; set; }
    public int QualityControlItemId { get; set; }
    public double QualityControlItemQty { get; set; }
    public bool QualityControlItemStatus { get; set; }
    public string QualityControlItemDescription { get; set; }
    public int? QualityControlSampleId { get; set; }
    public string QualityControlSampleCode { get; set; }
    public double? QualityControlSampleQty { get; set; }
    public QualityControlSampleStatus? QualityControlSampleStatus { get; set; }
    public byte[] QualityControlSampleRowVersion { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public long? StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }


    public int? UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public string DepartementName { get; set; }
    public string Departement { get; set; }
    public int? StatusChangerUserId { get; set; }
    public string StatusChangerEmployeeFullName { get; set; }
    public DateTime? DateTime { get; set; }
    public double? TestQty { get; set; }
    public double? ConsumeQty { get; set; }

  }
}
