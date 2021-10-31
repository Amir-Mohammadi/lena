using System;
using lena.Domains.Enums;
namespace lena.Models.Supplies.ProvisionersCartItem
{
  public class EditProvisionersCartItemInput
  {
    public int Id { get; set; }
    public Nullable<int> ProviderId { get; set; }
    public int PurchaseRequestId { get; set; }
    public double RequestQty { get; set; }
    public double SuppliedQty { get; set; }
    public byte[] RowVersion { get; set; }
  }
}