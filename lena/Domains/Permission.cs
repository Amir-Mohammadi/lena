using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Permission : IEntity
  {
    protected internal Permission()
    {
    }
    public int Id { get; set; }
    public int SecurityActionId { get; set; }
    public Nullable<int> UserGroupId { get; set; }
    public Nullable<int> UserId { get; set; }
    public AccessType AccessType { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual SecurityAction SecurityAction { get; set; }
    public virtual UserGroup UserGroup { get; set; }
    public virtual User User { get; set; }
  }
}
