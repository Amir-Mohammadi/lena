using System;
using System.Collections.Generic;
using lena.Models.Supplies.ProvisionersCartItemDetail;
using lena.Models.Supplies.PurchaseRequest;

using lena.Domains.Enums;
namespace lena.Models.Supplies.ProvisionersCartItem
{
  public class ProvisionersCartItemResult
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<int> ProviderId { get; set; }
    public string ProviderName { get; set; }
    public Nullable<double> RequestQty { get; set; }
    public Nullable<double> SuppliedQty { get; set; }
    public PurchaseRequestResult PurchaseRequest { get; set; }
    public int PurchaseRequestId { get; set; }
    public DateTime PurchaseRequestDeadline { get; set; }
    public double? PurchaseRequestQty { get; set; }
    public double? PurchaseRequestOrderedQty { get; set; }
    public int PurchaseRequestStuffId { get; set; }
    public int PurchaseRequestUnitId { get; set; }
    public Nullable<lena.Domains.Enums.ProvisionersCartItemStatus> Status { get; set; }
    public string PurchaseRequestStuffCode { get; set; }
    public string PurchaseRequestUnitName { get; set; }
    public string PurchaseRequestStuffName { get; set; }
    public IEnumerable<ProvisionersCartItemDetailResult> ProvisionersCartItemDetails { get; set; }
  }
}
