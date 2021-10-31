using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrder
{
  public class EditPurchaseOrderInput
  {
    public int Id { get; set; }
    public double Qty { get; set; }
    public double OrderedQty { get; set; }
    public byte UnitId { get; set; }
    public double? Price { get; set; }
    public byte? CurrencyId { get; set; }
    public int? ProviderId { get; set; }
    public int? SupplierId { get; set; }
    public DateTime BuyDeadline { get; set; }
    public PurchaseOrderType PurchaseOrderType { get; set; }
    public PurchaseOrderDetailInput[] PurchaseOrderDetail { get; set; }
    public AddPurchaseOrderInput[] NewAddedPurchaseOrders { get; set; }
    public byte[] RowVersion { get; set; }
  }
}