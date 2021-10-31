using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Cargo
{
  public class CargoResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public DateTime EstimateDateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public int HowToBuyId { get; set; }
    public string HowToBuyTitle { get; set; }
    public int? CurrentPurchaseStepId { get; set; }
    public int? CurrentPurchaseStepUserId { get; set; }
    public DateTime? CurrentPurchaseStepDateTime { get; set; }
    public DateTime? CurrentPurchaseStepFollowUpDateTime { get; set; }
    public string CurrentPurchaseStepEmployeeFullName { get; set; }
    public int? CurrentPurchaseStepHowToBuyDetailId { get; set; }
    public string CurrentPurchaseStepHowToBuyDetailTitle { get; set; }
    public string LatestBaseEntityDocumentDescription { get; set; }
  }
}
