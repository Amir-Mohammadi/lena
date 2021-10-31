using System;
using lena.Models.Supplies.ProvisionersCartItem;

using lena.Domains.Enums;
namespace lena.Models.Supplies.ProvisionersCart
{
  public class AddProvisionersCartInput
  {
    public AddProvisionersCartItemInput[] AddProvisionersCartItems { get; set; }
    public int SupplierId { get; set; }
    public Nullable<int> ResponsibleEmployeeId { get; set; }
    public string Description { get; set; }
    public DateTime ReportDate { get; set; }
  }
}
