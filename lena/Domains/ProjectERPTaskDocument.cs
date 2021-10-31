using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERPTaskDocument : IEntity
  {
    protected internal ProjectERPTaskDocument()
    { }
    public short Id { get; set; }
    public Guid DocumentId { get; set; }
    public int ProjectERPTaskId { get; set; }
    public int CreatorUserId { get; set; }
    public DateTime CreateDateTime { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User CreatorUser { get; set; }
    public virtual ProjectERPTask ProjectERPTask { get; set; }
  }
}
