using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BaseEntityDocument : IEntity
  {
    protected internal BaseEntityDocument()
    {
    }
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime DateTime { get; set; }
    public int BaseEntityId { get; set; }
    public int UserId { get; set; }
    public Nullable<Guid> DocumentId { get; set; }
    public Nullable<int> BaseEntityDocumentTypeId { get; set; }
    public Nullable<int> CooperatorId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual BaseEntity BaseEntity { get; set; }
    public virtual User User { get; set; }
    public virtual BaseEntityDocumentType BaseEntityDocumentType { get; set; }
    public virtual Cooperator Cooperator { get; set; }
  }
}
