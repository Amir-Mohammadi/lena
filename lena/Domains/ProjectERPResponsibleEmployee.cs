using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERPResponsibleEmployee : IEntity
  {
    protected internal ProjectERPResponsibleEmployee()
    { }
    public int ProjectERPId { get; set; }
    public int ResponsibleEmployeeId { get; set; }
    public DateTime CreatorDateTime { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Employee ResponsibleEmployee { get; set; }
    public virtual ProjectERP ProjectERP { get; set; }
  }
}
