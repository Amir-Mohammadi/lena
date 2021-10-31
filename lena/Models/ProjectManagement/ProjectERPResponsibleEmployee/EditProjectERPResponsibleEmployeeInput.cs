using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPResponsibleEmployee
{
  public class EditProjectERPResponsibleEmployeeInput
  {
    public int ProjectERPId { get; set; }
    public int ResponsibleEmployeeId { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
