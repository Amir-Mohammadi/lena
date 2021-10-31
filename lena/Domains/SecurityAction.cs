using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class SecurityAction : IEntity
  {
    protected internal SecurityAction()
    {
      this.Permissions = new HashSet<Permission>();
      this.ActionParamaters = new HashSet<ActionParameter>();
      this.PermissionRequestActions = new HashSet<PermissionRequestAction>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string ActionName { get; set; }
    public byte[] RowVersion { get; set; }
    public int SecurityActionGroupId { get; set; }
    public bool IsPublicAction { get; set; }
    public virtual ICollection<Permission> Permissions { get; set; }
    public virtual ICollection<ActionParameter> ActionParamaters { get; set; }
    public virtual SecurityActionGroup SecurityActionGroup { get; set; }
    public virtual ICollection<PermissionRequestAction> PermissionRequestActions { get; set; }
  }
}