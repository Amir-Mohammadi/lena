using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EmployeeComplain : IEntity
  {
    protected internal EmployeeComplain()
    {
      this.EmployeeComplainItems = new HashSet<EmployeeComplainItem>();
    }
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual ICollection<EmployeeComplainItem> EmployeeComplainItems { get; set; }
  }
}
