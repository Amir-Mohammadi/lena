using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OrderDocument : IEntity, IHasDocument, IRemovable
  {
    public int OrderId { get; set; }
    public Guid DocumentId { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Order Order { get; set; }
    public virtual User User { get; set; }
    public virtual Document Document { get; set; }
  }
}
