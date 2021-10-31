using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StoreReceiptSerialProfile : SerialProfile, IEntity
  {
    protected internal StoreReceiptSerialProfile()
    {
    }
    public virtual StoreReceipt StoreReceipt { get; set; }
    public int StoreReceiptId { get; internal set; }
  }
}