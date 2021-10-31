using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BaseEntityConfirmType : IEntity
  {
    protected internal BaseEntityConfirmType()
    {
      this.BaseEntityConfirmations = new HashSet<BaseEntityConfirmation>();
    }
    public short Id { get; set; }
    public short DepartmentId { get; set; }
    public Nullable<int> UserId { get; set; }
    public string ConfirmPageUrl { get; set; }
    public EntityType ConfirmType { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Department Department { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<BaseEntityConfirmation> BaseEntityConfirmations { get; set; }
  }
}