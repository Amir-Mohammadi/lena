using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class OrganizationJob : IEntity
  {
    public OrganizationJob()
    {
      this.Employees = new HashSet<Employee>();
    }
    public int Id { get; set; }
    //طبقه بندی مشاغل و ...
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int? OranizationPostId { get; set; }
    public virtual OrganizationPost OrganizationPost { get; set; }
    public int CreatorId { get; set; }
    public User Creator { get; set; }
    public DateTime CreationTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<Employee> Employees { get; set; }
  }
}
