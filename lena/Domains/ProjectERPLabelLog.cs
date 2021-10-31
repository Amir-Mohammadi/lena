using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERPLabelLog : IEntity
  {
    protected internal ProjectERPLabelLog()
    {
    }
    public int ProjectERPId { get; set; }
    public short ProjectERPLabelId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProjectERP ProjectERP { get; set; }
    public virtual ProjectERPLabel ProjectERPLabel { get; set; }
  }
}
