using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PermissionRequestAction : IEntity
  {
    protected internal PermissionRequestAction()
    {
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int PermissionRequestId { get; set; }
    public string Description { get; set; }
    public PermissionRequestActionStatus Status { get; set; }
    public Nullable<int> ConfirmationUserId { get; set; }
    public Nullable<int> SecurityActionId { get; set; }
    public Nullable<AccessType> AccessType { get; set; }
    public virtual PermissionRequest PermissionRequest { get; set; }
    public virtual User ConfirmationUser { get; set; }
    public virtual SecurityAction SecurityAction { get; set; }
  }
}