using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PurchasePrice : StuffPrice, IEntity
  {
    protected internal PurchasePrice()
    {
    }
    public int StoreReceiptId { get; set; }
    public double CurrencyRate { get; set; }
    public double RialPrice { get; set; }
    public double TransferCost { get; set; }
    public double DutyCost { get; set; }
    public double OtherCost { get; set; }
    public double Discount { get; set; }
    public virtual StoreReceipt StoreReceipt { get; set; }
    public virtual StoreReceipt ActiveForStoreReceipt { get; set; }
  }
}