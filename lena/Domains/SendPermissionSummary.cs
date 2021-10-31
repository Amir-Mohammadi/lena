using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class SendPermissionSummary : IEntity
  {
    protected internal SendPermissionSummary()
    {
    }
    public int Id { get; set; }
    public double PreparingSendingQty { get; set; }
    public double SendedQty { get; set; }
    public int SendPermissionId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual SendPermission SendPermission { get; set; }
  }
}