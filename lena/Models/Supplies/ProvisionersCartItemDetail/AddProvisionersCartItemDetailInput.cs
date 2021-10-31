using lena.Models.Supplies.PurchaseOrderGroup;
using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.ProvisionersCartItemDetail
{
  public class AddProvisionersCartItemDetailInput
  {
    public int ProviderId { get; set; }
    public double SupplyQty { get; set; }
    public string Description { get; set; }
    public int ProvisionersCartItemId { get; set; }
    public byte[] RowVersion { get; set; }
    public int UnitPrice { get; set; }
    public byte CurrencyId { get; set; }
    public DateTime PurchaseOrderPreparingDateTime { get; set; }
    public int? SupplierId { get; set; }
    public int PurchaseRequestId { get; set; }
    public int StuffId { get; set; }
    public byte UnitId { get; set; }
    public DateTime Deadline { get; set; }
    public double? Price { get; set; }
    public double Qty { get; set; }
    public PurchaseOrderGroupItemInput[] PurchaseOrderGroupItems { get; set; }
  }
}
