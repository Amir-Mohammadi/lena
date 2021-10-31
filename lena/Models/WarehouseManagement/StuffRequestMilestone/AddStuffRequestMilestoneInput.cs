using System;
using lena.Models.WarehouseManagement.StuffRequestMilestoneDetail;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequestMilestone
{
  public class AddStuffRequestMilestoneInput
  {
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public int StuffRequestMilestoneId { get; set; }
    public AddStuffRequestMilestoneDetailInput[] StuffRequestMilestoneDetails { get; set; }
  }
}
