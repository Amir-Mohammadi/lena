using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PreparingSending : BaseEntity, IEntity
  {
    protected internal PreparingSending()
    {
      this.PreparingSendingItems = new HashSet<PreparingSendingItem>();
    }
    public int SendPermissionId { get; set; }
    public PreparingSendingStatus Status { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public byte[] RowVersion { get; set; }

    public virtual SendPermission SendPermission { get; set; }
    public virtual SendProduct SendProduct { get; set; }
    public virtual ICollection<PreparingSendingItem> PreparingSendingItems { get; set; }
    public virtual Unit Unit { get; set; }
  }
}
