using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class OrganizationPost : IEntity
  {
    protected internal OrganizationPost()
    {
      this.OrganizationPosts = new HashSet<OrganizationPost>();
      this.OrganizationJobs = new HashSet<OrganizationJob>();
    }
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public OrganizationPost Parent { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsAdmin { get; set; }
    public int CreatorId { get; set; }
    public User Creator { get; set; }
    public DateTime CreationTime { get; set; }
    public int? UserGroupId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual UserGroup UserGroup { get; set; }
    public virtual ICollection<Employee> Employees { get; set; }
    public virtual ICollection<OrganizationPostHistory> PostHistories { get; set; }
    public virtual DepartmentManager DepartmentManager { get; set; }
    public virtual ICollection<OrganizationPost> OrganizationPosts { get; set; }
    public virtual ICollection<OrganizationJob> OrganizationJobs { get; set; }
  }
}