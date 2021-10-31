using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseStep
{
  public class AddPurchaseStepInput
  {
    public string Description { get; set; }
    public int HowToBuyDetailId { get; set; }
    public int CargoId { get; set; }
    public int CargoItemId { get; set; }
    public DateTime FollowUpDateTime { get; set; }
    public byte[] CargoRowVersion { get; set; }
  }
}
