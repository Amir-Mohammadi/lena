using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SendProduct
{
  public class SendProductFullResult
  {
    public int Id { get; set; }
    public int ExitReceiptId { get; set; }
    public string Code { get; set; }
    public int PreparingSendingId { get; set; }
    public string PreparingSendingCode { get; set; }
    public int SendPermissionId { get; set; }
    public string SendPermissionCode { get; set; }
    public int CooperatorId { get; set; }
    public string CooperatorCode { get; set; }
    public string CooperatorName { get; set; }
    public int? OrderItemId { get; set; }
    public string OrderItemCode { get; set; }
    public int? OrderItemUnitId { get; set; }
    public string OrderItemUnitName { get; set; }
    public double? OrderItemQty { get; set; }
    public int ExitReceiptRequestId { get; set; }
    public double ExitReceiptRequestQty { get; set; }
    public int ExitReceiptRequestUnitId { get; set; }
    public string ExitReceiptRequestUnitName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double SendPermissionQty { get; set; }
    public int SendPermissionUnitId { get; set; }
    public string SendPermissionUnitName { get; set; }
    public DateTime DateTime { get; set; }
    public PreparingSendingStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int ExitReceiptRequestTypeId { get; set; }
    public string ExitReceiptRequestTypeTitle { get; set; }
  }
}
