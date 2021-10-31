using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ReturnStoreReceipt : StoreReceipt, IEntity
  {
    protected internal ReturnStoreReceipt()
    {
      this.ReturnOfSales = new HashSet<ReturnOfSale>();
      this.ReturnedExitReceiptRequests = new HashSet<ReturnedExitReceiptRequest>();
    }
    public virtual ICollection<ReturnOfSale> ReturnOfSales { get; set; }
    public virtual ICollection<ReturnedExitReceiptRequest> ReturnedExitReceiptRequests { get; set; }
  }
}