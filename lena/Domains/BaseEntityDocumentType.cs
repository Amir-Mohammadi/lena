using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BaseEntityDocumentType : IEntity
  {
    protected internal BaseEntityDocumentType()
    {
      this.BaseEntityDocuments = new HashSet<BaseEntityDocument>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Nullable<EntityType> EntityType { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<BaseEntityDocument> BaseEntityDocuments { get; set; }
  }
}