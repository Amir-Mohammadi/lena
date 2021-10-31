using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class OrganizationPostHistory : IEntity
  {
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public int OrganizationPostId { get; set; }
    public OrganizationPost OrganizationPost { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime CreationTime { get; set; }
    public int CreatorId { get; set; }
    public User Creator { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
