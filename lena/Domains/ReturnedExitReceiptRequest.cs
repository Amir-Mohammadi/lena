using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ReturnedExitReceiptRequest : ExitReceiptRequest, IEntity
  {
    protected internal ReturnedExitReceiptRequest()
    {
    }
    public int ReturnStoreReceiptId { get; set; }
    public virtual ReturnStoreReceipt ReturnStoreReceipt { get; set; }
  }
}