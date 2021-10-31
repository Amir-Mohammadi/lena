using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrder
{
  public class AddPurchaseOrderInput
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public int PurchaseRequestId { get; set; }
    public double Qty { get; set; }
    public double OrderedQty { get; set; }
    public byte UnitId { get; set; }
    public double? Price { get; set; }
    public byte? CurrencyId { get; set; }
    public int? ProviderId { get; set; }
    public string OrderInvoiceNum { get; set; }
    public int? SupplierId { get; set; }
    public int PurchaseOrderGroupId { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime PurchaseOrderDateTime { get; set; }
    public DateTime PurchaseOrderPreparingDateTime { get; set; }
    public double? CurrentStuffBasePrice { get; set; }
    public byte? CurrentStuffBasePriceCurrencyId { get; set; }
    public string StuffPriceDiscrepancyDescription { get; set; }
  }
}