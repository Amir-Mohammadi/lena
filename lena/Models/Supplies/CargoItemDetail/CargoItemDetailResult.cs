using System;
using System.Linq;
using lena.Domains.Enums;
using lena.Models.Supplies.LadingItemDetail;
using lena.Models.Supplies.Ladings;

using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItemDetail
{
  public class CargoItemDetailResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public double? Qty { get; set; }
    public double? ReceiptQty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public byte[] RowVersion { get; set; }
    public IQueryable<LadingItemResult> LadingItems { get; set; }
    public double? SumLadingItemsQty { get; set; }
    public double? LadingItemQty { get; set; }
    public double? LadingItemDetailQty { get; set; }
    public IQueryable<LadingItemDetailResult> LadingItemDetails { get; set; }
    public double? SumLadingItemDetailsQty { get; set; }

    public int CargoId { get; set; }
    public string CargoCode { get; set; }

    public int CargoItemId { get; set; }
    public string CargoItemCode { get; set; }
    public int CargoItemStuffId { get; set; }
    public string CargoItemStuffCode { get; set; }
    public string CargoItemStuffName { get; set; }
    public double? CargoItemQty { get; set; }
    public double? CargoItemReceiptQty { get; set; }
    public int CargoItemUnitId { get; set; }
    public string CargoItemUnitName { get; set; }
    public CargoItemStatus CargoItemStatus { get; set; }
    public Nullable<int> PurchaseRequestId { get; set; }

    public int CargoItemHowToBuyId { get; set; }
    public DateTime CargoItemEstimateDateTime { get; set; }
    public DateTime CargoItemDateTime { get; set; }
    public byte[] CargoItemRowVersion { get; set; }

    public int PurchaseOrderId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public int PurchaseOrderStuffId { get; set; }
    public string PurchaseOrderStuffCode { get; set; }
    public string PurchaseOrderStuffName { get; set; }

    public int? PurchaseOrderProviderId { get; set; }
    public string PurchaseOrderProviderName { get; set; }
    public int PurchaseOrderUnitId { get; set; }
    public string PurchaseOrderUnitName { get; set; }
    public double? PurchaseOrderQty { get; set; }
    public double? PurchaseOrderPrice { get; set; }
    public int? PurchaseOrderCurrencyId { get; set; }
    public string PurchaseOrderCurrencyTitle { get; set; }
    public double? PurchaseOrderReceiptedQty { get; set; }
    public double? PurchaseOrderCargoedQty { get; set; }
    public double? PurchaseOrderRemainedQty { get; set; }
    public DateTime PurchaseOrderDeadline { get; set; }

    public double? PurchaseOrderTotalWeight { get; set; }
    public int PurchaseOrderDetailId { get; set; }
    public int PurchaseOrderDetailUnitId { get; set; }
    public string PurchaseOrderDetailUnitName { get; set; }
    public double? PurchaseOrderDetailQty { get; set; }
    public double? PurchaseOrderDetailCargoedQty { get; set; }
    public double? PurchaseOrderDetailReceiptedQty { get; set; }
    public double? PurchaseOrderDetailRemainedQty { get; set; }
    public double? PurchaseOrderDetailQualityControlPassedQty { get; set; }
    public int? ForwarderId { get; set; }
    public string ForwarderName { get; set; }
    public Guid? ForwarderDocumentId { get; set; }
    public BuyingProcess? BuyingProcess { get; set; }
    public int DecimalDigitCount { get; set; }
  }
}
