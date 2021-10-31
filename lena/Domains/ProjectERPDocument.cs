using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERPDocument : IEntity
  {
    protected internal ProjectERPDocument()
    {
    }
    public short Id { get; set; }
    public Guid DocumentId { get; set; }
    public int UserId { get; set; }
    public int ProjectERPId { get; set; }
    public short ProjectERPDocumentTypeId { get; set; }
    public DateTime CreateDateTime { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    //public string FileName { get; set; }
    //public string FileFormat { get; set; }
    //public double FileSize { get; set; }    
    public virtual User User { get; set; }
    public virtual ProjectERP ProjectERP { get; set; }
    public virtual ProjectERPDocumentType ProjectERPDocumentType { get; set; }
  }
}