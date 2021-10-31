using lena.Domains.Enums;
namespace lena.Models.UserManagement.OrganizationJobs
{
  public class EditOrganizationJobInput : AddOrganizationJobInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
