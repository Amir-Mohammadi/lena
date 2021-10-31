using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ScrumEntityDocument : IEntity
  {
    protected internal ScrumEntityDocument()
    {
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ScrumEntityId { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ScrumEntity ScrumEntity { get; set; }
    public virtual DocumentType DocumentType { get; set; }
  }
}