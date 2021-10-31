using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class SendPermission : BaseEntity, IEntity
  {
    protected internal SendPermission()
    {
      this.PreparingSendings = new HashSet<PreparingSending>();
    }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public SendPermissionStatusType SendPermissionStatusType { get; set; }
    public int ExitReceiptRequestId { get; set; }
    public Nullable<DateTime> ConfirmDate { get; set; }
    public Nullable<int> ConfirmerUserId { get; set; }
    public string ConfirmDescription { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<PreparingSending> PreparingSendings { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual SendPermissionSummary SendPermissionSummary { get; set; }
    public virtual ExitReceiptRequest ExitReceiptRequest { get; set; }
    public virtual User Confirmer { get; set; }
  }
}