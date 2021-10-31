using System;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.OrganizationJobs
{
  public class OrganizationJobResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int CreatorId { get; set; }
    public int? OrganizationPostId { get; set; }
    public string OrganizationPostName { get; set; }
    public string CreatorFullName { get; set; }
    public DateTime CreationTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
