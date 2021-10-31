using lena.Domains.Enums;
namespace lena.Models.UserManagement.OrganizationJobs
{
  public class AddOrganizationJobInput
  {
    public int? ParentId { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int? OrganizationPostId { get; set; }
  }
}
