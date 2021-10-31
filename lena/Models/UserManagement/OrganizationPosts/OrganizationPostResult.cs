using System;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.OrganizationPosts
{
  public class OrganizationPostResult
  {
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string ParentName { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsAdmin { get; set; }
    public int CreatorId { get; set; }
    public int? UserGroupId { get; set; }
    public string UserGroupName { get; set; }
    public string CreatorFullName { get; set; }
    public DateTime CreationTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
