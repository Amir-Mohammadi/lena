using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrderDetail
{
  public class PurchaseOrderDetailResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double ConversionRatio { get; set; }
    public double CargoedQty { get; set; }
    public double ReceiptedQty { get; set; }
    public double RemainedQty { get; set; }
    public double QualityControlPassedQty { get; set; }
    public double TotalWeight { get; set; }
    public byte[] RowVersion { get; set; }

    public int? PurchaseRequestId { get; set; }
    public string PurchaseRequestCode { get; set; }
    public string PurchaseRequestDepartmentName { get; set; }
    public string PurchaseRequestEmployeeFullName { get; set; }
    public string PurchaseRequestResponsibleEmployeeFullName { get; set; }
    public DateTime? PurchaseRquestDeadline { get; set; }
    public double? PurchaseRequestQty { get; set; }
    public int? PurchaseRequestUnitId { get; set; }
    public string PurchaseRequestUnitName { get; set; }
    public double? PurchaseRequestCargoedQty { get; set; }
    public double? PurchaseRequestReceiptedQty { get; set; }
    public double? PurchaseRequsetRemainedQty { get; set; }
    public DateTime? PurchaseRequestDateTime { get; set; }
    public PurchaseRequestStatus? PurchaseRequestStatus { get; set; }

    public int PurchaseOrderDetailStuffId { get; set; }
    public string PurchaseOrderDetailStuffCode { get; set; }
    public string PurchaseOrderDetailStuffName { get; set; }
    public double PurchaseOrderDetailQty { get; set; }
    public int PurchaseOrderDetailUnitId { get; set; }
    public string PurchaseOrderDetailUnitName { get; set; }
    public int? PurchaseOrderDetailProviderId { get; set; }
    public string PurchaseOrderDetailProviderName { get; set; }
    public double PurchaseOrderDetailCargoedQty { get; set; }
    public double PurchaseOrderDetailReceiptQty { get; set; }
    public double PurchaseOrderDetailRemainedQty { get; set; }
    public DateTime PurchaseOrderDetailDeadline { get; set; }
    public CargoItemStatus CargoItemStatus { get; set; }
    public string PlanCode { get; set; }
    public string PurchaseRequsetDescription { get; set; }
    public int DecimalDigitCount { get; set; }
  }
}
