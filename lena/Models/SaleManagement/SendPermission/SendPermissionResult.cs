using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.SendPermission
{
  public class SendPermissionResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int? OrderItemId { get; set; }
    public string OrderItemCode { get; set; }
    public double? OrderItemQty { get; set; }
    public int? OrderItemBlockId { get; set; }
    public string OrderItemBlockCode { get; set; }
    public double? OrderItemBlockQty { get; set; }
    public int OrderItemBlockUnitId { get; set; }
    public string OrderItemBlockUnitName { get; set; }
    public int StuffId { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public double? Qty { get; set; }
    public double? Price { get; set; }
    public double? TotalPrice { get; set; }
    public string CurrencyTitle { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public DateTime DateTime { get; set; }
    public bool? Confirmed { get; set; }
    public int? OrderItemUnitId { get; set; }
    public string OrderItemUnitName { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerCode { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public double? SendedQty { get; set; }
    public double? PreparingSendingQty { get; set; }
    public int ExitReceiptRequestId { get; set; }
    public string ExitReceiptRequestCode { get; set; }
    public double ExitReceiptRequestQty { get; set; }
    public int ExitReceiptRequestUnitId { get; set; }
    public string ExitReceiptRequestUnitName { get; set; }
    public int ExitReceiptRequestTypeId { get; set; }
    public string ExitReceiptRequestTypeTitle { get; set; }
    public ExitReceiptRequestStatus OrderItemBlockStatusType { get; set; }
    public ExitReceiptRequestStatus ExitReceiptRequestStatus { get; set; }
    public SendPermissionStatusType SendPermissionStatusType { get; set; }
    public OrderItemBlockType OrderItemBlockType { get; set; }
    public string EmployeeFullName { get; set; }
    public string ConfirmerFullName { get; set; }
    public DateTime? ConfirmDate { get; set; }
    public string ConfirmDescription { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
