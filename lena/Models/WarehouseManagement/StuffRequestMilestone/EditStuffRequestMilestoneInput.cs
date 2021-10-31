using System;
using lena.Models.WarehouseManagement.StuffRequestMilestoneDetail;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequestMilestone
{
  public class EditStuffRequestMilestoneInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public AddStuffRequestMilestoneDetailInput[] AddStuffRequestMilestoneDetails { get; set; }
    public EditStuffRequestMilestoneDetailInput[] EditStuffRequestMilestoneDetails { get; set; }
    public DeleteStuffRequestMilestoneDetailInput[] DeleteStuffRequestMilestoneDetails { get; set; }

  }
}
