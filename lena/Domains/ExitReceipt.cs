using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ExitReceipt : BaseEntity, IEntity
  {
    protected internal ExitReceipt()
    {
      this.SendProducts = new HashSet<SendProduct>();
      this.ExitReceiptDeleteRequests = new HashSet<ExitReceiptDeleteRequest>();
    }
    public Nullable<bool> Confirmed { get; set; }
    public Nullable<int> OutboundCargoId { get; set; }
    public int CooperatorId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<SendProduct> SendProducts { get; set; }
    public virtual ICollection<ExitReceiptDeleteRequest> ExitReceiptDeleteRequests { get; set; }
    public virtual OutboundCargo OutboundCargo { get; set; }
    public virtual Cooperator Cooperator { get; set; }
  }
}