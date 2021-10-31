using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ExitReceiptRequestSummary : IEntity
  {
    protected internal ExitReceiptRequestSummary()
    {
    }
    public int Id { get; set; }
    public double PermissionQty { get; set; }
    public double PreparingSendingQty { get; set; }
    public double SendedQty { get; set; }
    public int ExitReceiptRequestId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ExitReceiptRequest ExitReceiptRequest { get; set; }
  }
}