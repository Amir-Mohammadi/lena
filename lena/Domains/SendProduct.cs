using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class SendProduct : BaseEntity, IEntity
  {
    protected internal SendProduct()
    {
      this.ReturnOfSales = new HashSet<ReturnOfSale>();
    }
    public int ExitReceiptId { get; set; }
    public int PreparingSendingId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ExitReceipt ExitReceipt { get; set; }
    public virtual PreparingSending PreparingSending { get; set; }
    public virtual ICollection<ReturnOfSale> ReturnOfSales { get; set; }
  }
}