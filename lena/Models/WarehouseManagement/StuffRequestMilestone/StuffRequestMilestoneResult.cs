using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequestMilestone
{
  public class StuffRequestMilestoneResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public Boolean IsDelete { get; set; }

    public int UserId { get; set; }
    public bool IsClosed { get; set; }

    public string EmployeeFullName { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime DueDate { get; set; }

    public StuffRequestMilestoneStatus Status { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
