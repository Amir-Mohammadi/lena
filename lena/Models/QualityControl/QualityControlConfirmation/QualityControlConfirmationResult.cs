using lena.Domains.Enums;
using lena.Models.QualityControl.QualityControlConfirmationTest;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlConfirmation
{
  public class QualityControlConfirmationResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int StuffId { get; set; }
    public int QualityControlId { get; set; }
    public string QualityControlCode { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public QualityControlType QualityControlType { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public QualityControlStatus Status { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double? AcceptedQty { get; set; }
    public double? FailedQty { get; set; }
    public double? ConditionalRequestQty { get; set; }
    public double? ConditionalQty { get; set; }
    public double? ReturnedQty { get; set; }
    public double? ConsumedQty { get; set; }
    public string QualityControlDescription { get; set; }
    public string Description { get; set; }
    public IQueryable<QualityControlConfirmationTestResult> QualityControlConfirmationTests { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
