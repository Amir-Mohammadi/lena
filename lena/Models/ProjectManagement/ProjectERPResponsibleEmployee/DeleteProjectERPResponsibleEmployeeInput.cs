using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPResponsibleEmployee
{
  public class DeleteProjectERPResponsibleEmployeeInput
  {
    public int ProjectERPId { get; set; }
    public int ResponsibleEmployeeId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
