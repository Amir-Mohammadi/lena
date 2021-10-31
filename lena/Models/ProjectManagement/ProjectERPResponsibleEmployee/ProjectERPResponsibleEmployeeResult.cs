using System;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPResponsibleEmployee
{
  public class ProjectERPResponsibleEmployeeResult
  {
    public int ProjectERPId { get; set; }
    public string ProjectERPTitle { get; set; }
    public int ResponsibleEmployeeId { get; set; }
    public string ResponsibleEmployeeFullName { get; set; }
    public DateTime CreatorDateTime { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }

  }
}