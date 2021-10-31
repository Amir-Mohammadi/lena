using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERPEventDocument : IEntity
  {
    protected internal ProjectERPEventDocument()
    {
    }
    public short Id { get; set; }
    //public string FileName { get; set; }
    //public string FileFormat { get; set; }
    //public string FileSize { get; set; }
    //public string FileContent { get; set; }
    public string Description { get; set; }
    public int ProjectERPEventId { get; set; }
    public Guid DocumentId { get; set; }
    public int CreatorUserId { get; set; }
    public DateTime CreationDateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User CreatorUser { get; set; }
    public virtual ProjectERPEvent ProjectERPEvent { get; set; }
  }
}
