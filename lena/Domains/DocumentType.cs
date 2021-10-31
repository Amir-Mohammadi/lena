using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class DocumentType : IEntity
  {
    protected internal DocumentType()
    {
      this.ScrumEntityDocuments = new HashSet<ScrumEntityDocument>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ScrumEntityDocument> ScrumEntityDocuments { get; set; }
  }
}
