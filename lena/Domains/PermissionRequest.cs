using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PermissionRequest : IEntity
  {
    protected internal PermissionRequest()
    {
      this.PermissionRequestActions = new HashSet<PermissionRequestAction>();
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<int> RegistrarUserId { get; set; }
    public DateTime RegisterDateTime { get; set; }
    public Nullable<int> IntendedUserId { get; set; }
    public virtual User RegistrarUser { get; set; }
    public virtual User IntendedUser { get; set; }
    public virtual ICollection<PermissionRequestAction> PermissionRequestActions { get; set; }
  }
}