using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPResponsibleEmployee
{
  public class AddProjectERPResponsibleEmployeeInput
  {
    public int ProjectERPId { get; set; }
    public int ResponsibleEmployeeId { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
  }
}
