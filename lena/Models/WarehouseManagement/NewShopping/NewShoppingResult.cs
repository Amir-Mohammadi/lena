using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.NewShopping
{
  public class NewShoppingResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int? ReceiptId { get; set; }
    public string ReceiptCode { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int InboundCargoId { get; set; }
    public string InboundCargoCode { get; set; }
    public int CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int BoxNo { get; set; }
    public double QtyPerBox { get; set; }
    public int? PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public int? CargoId { get; set; }
    public string CargoCode { get; set; }
    public int? CargoItemId { get; set; }
    public string CargoItemCode { get; set; }

    public int LadingId { get; set; }
    public string LadingCode { get; set; }
    public int? LadingItemId { get; set; }

    public StoreReceiptType StoreReceiptType { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime? ReceiptDateTime { get; set; }
    public ReceiptStatus? ReceiptStatus { get; set; }

    public int? BillOfMaterialVersion { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
