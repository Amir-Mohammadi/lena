using lena.Domains.Enums;
using lena.Models.WarehouseManagement.StuffRequestMilestoneDetail;
using System;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequestMilestone
{
  public class FullStuffRequestMilestoneResult
  {
    public int Id { get; set; }
    public string Code { get; set; }

    public int UserId { get; set; }

    public string EmployeeFirstName { get; set; }
    public string EmployeeLastName { get; set; }
    public string EmployeeFullName { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime DueDate { get; set; }

    public StuffRequestMilestoneStatus Status { get; set; }
    public IQueryable<StuffRequestMilestoneDetailResult> StuffRequestMilestoneDetails { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
