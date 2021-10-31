using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class DepartmentManager : IEntity
  {
    public int Id { get; set; }
    public int? UserId { get; set; }
    public DateTime? DateTime { get; set; }
    public int? OrganizationPostId { get; set; }
    public short? DepartmentId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual OrganizationPost OrganizationPost { get; set; }
    public virtual Department Department { get; set; }
  }
}