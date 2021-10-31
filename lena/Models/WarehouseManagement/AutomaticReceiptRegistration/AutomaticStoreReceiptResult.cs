using lena.Domains.Enums;
using System;
using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.AutomaticReceiptRegistration
{
  public class AutomaticStoreReceiptResult
  {
    public int Id { get; set; }
    public StoreReceiptType StoreReceiptType { get; set; }
    public string Code { get; set; }
    public int? ReceiptId { get; set; }
    public string ReceiptCode { get; set; }
    public int ProviderId { get; set; }
    public string ProviderName { get; set; }
    public int LandingId { get; set; }
    public string LadingCode { get; set; }
    public string CargoCode { get; set; }
    public int? CargoItemId { get; set; }
    public string CargoItemCode { get; set; }
    public int StuffId { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public string InboundCargoCode { get; set; }
    public int InboundCargoId { get; set; }
    public string PurchaseOrderCode { get; set; }
    public double QtyPerBox { get; set; }
    public int BoxNo { get; set; }
    public DateTime TransportDateTime { get; set; }
    public ReceiptStatus ReceiptStatus { get; set; }
    public IEnumerable<QualityControlStatus> QualityControlStatus { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
