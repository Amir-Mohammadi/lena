using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.ProvisionersCartItem
{
  public class AddProvisionersCartItemInput
  {
    public Nullable<int> ProviderId { get; set; }
    public int PurchaseRequestId { get; set; }
    public double RequestQty { get; set; }
    public double SuppliedQty { get; set; }
  }
}
