using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.ProvisionersCartItemDetail
{
  public class ProvisionersCartItemDetailResult
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int UnitPrice { get; set; }
    public int CurrencyId { get; set; }
    public string Description { get; set; }
    public Nullable<int> ProviderId { get; set; }
    public string ProviderName { get; set; }
    public string PurchaseOrderCode { get; set; }
    public Nullable<double> SupplyQty { get; set; }
    public Nullable<System.DateTime> DateTime { get; set; }
    public Nullable<int> ProvisionersCartItemId { get; set; }
    public string CurrencyTitle { get; set; }
  }
}