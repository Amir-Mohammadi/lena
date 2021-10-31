using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ReceiptQualityControl : QualityControl, IEntity
  {
    protected internal ReceiptQualityControl()
    {
    }
    public int StoreReceiptId { get; set; }
    public virtual StoreReceipt StoreReceipt { get; set; }
  }
}