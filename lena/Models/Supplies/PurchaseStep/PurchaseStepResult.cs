using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseStep
{
  public class PurchaseStepResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int CargoItemId { get; set; }
    public string CargoCode { get; set; }
    public int UserId { get; set; }
    public string EmployeeName { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public int HowToBuyDetailId { get; set; }
    public int HowToBuyDetailOrder { get; set; }
    public string HowToBuyDetailTitle { get; set; }
    public DateTime FollowUpDateTime { get; set; }
    public int HowToBuyId { get; set; }
    public string HowToBuyTitle { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
