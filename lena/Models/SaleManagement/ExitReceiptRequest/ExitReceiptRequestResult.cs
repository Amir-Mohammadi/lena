using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.ExitReceiptRequest
{
  public class ExitReceiptRequestResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int ExitReceiptRequestTypeId { get; set; }
    public string ExitReceiptRequestTypeTitle { get; set; }
    public ExitReceiptRequestStatus Status { get; set; }
    public double PermissionQty { get; set; }
    public double PreparingSendingQty { get; set; }
    public double? OrderItemQty { get; set; }
    public double SendedQty { get; set; }
    public int CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public string CooperatorCode { get; set; }
    public string Address { get; set; }
    public int? UserEmployeeId { get; set; }
    public string UserEmployeeFullName { get; set; }
    public int? OrderItemId { get; set; }
    public string RequestCode { get; set; }
    public bool AutoConfirm { get; set; }
    public string OrderItemCode { get; set; }
    public int? PurchaseOrderId { get; set; }
    public double? PurchaseOrderQty { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
