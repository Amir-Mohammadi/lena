using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Receipt : BaseEntity, IEntity
  {
    protected internal Receipt()
    {
      this.StoreReceipts = new HashSet<StoreReceipt>();
    }
    public int CooperatorId { get; set; }
    public ReceiptStatus Status { get; set; }
    public DateTime ReceiptDateTime { get; set; }
    public StoreReceiptType ReceiptType { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<StoreReceipt> StoreReceipts { get; set; }
    public virtual Cooperator Cooperator { get; set; }
  }
}