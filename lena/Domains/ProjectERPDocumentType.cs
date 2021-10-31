using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProjectERPDocumentType : IEntity
  {
    protected internal ProjectERPDocumentType()
    {
      this.ProjectERPDocuments = new HashSet<ProjectERPDocument>();
    }
    public short Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateDateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ProjectERPDocument> ProjectERPDocuments { get; set; }
  }
}
