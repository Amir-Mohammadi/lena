using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.PreparingSending
{
  public class PreparingSendingResult
  {

    public int Id { get; set; }
    public string Code { get; set; }
    public int SendPermissionId { get; set; }
    public string SendPermissionCode { get; set; }
    public int CustomerId { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public int? OrderItemId { get; set; }
    public string OrderItemCode { get; set; }
    public double OrderItemQty { get; set; }
    public int? OrderItemBlockId { get; set; }
    public string OrderItemBlockCode { get; set; }
    public double OrderItemBlockQty { get; set; }
    public int OrderItemBlockUnitId { get; set; }
    public string OrderItemBlockUnitName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double UnitConversionRatio { get; set; }
    public DateTime DateTime { get; set; }
    public int? ExitReceiptId { get; set; }
    public DateTime? ExitReceiptDateTime { get; set; }
    public bool? ExitReceiptConfirm { get; set; }
    public int? SendProductId { get; set; }
    public string SendProductCode { get; set; }
    public string ExitReceiptCode { get; set; }
    public int? OrderItemUnitId { get; set; }
    public string OrderItemUnitName { get; set; }
    public PreparingSendingStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public double SendPermissionQty { get; set; }
    public int SendPermissionUnitId { get; set; }
    public string SendPermissionUnitName { get; set; }
  }
}
