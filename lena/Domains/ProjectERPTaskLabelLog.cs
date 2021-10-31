using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERPTaskLabelLog : IEntity
  {
    protected internal ProjectERPTaskLabelLog()
    {
    }
    public int ProjectERPTaskId { get; set; }
    public short ProjectERPLabelId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProjectERPTask ProjectERPTask { get; set; }
    public virtual ProjectERPLabel ProjectERPLabel { get; set; }
  }
}
