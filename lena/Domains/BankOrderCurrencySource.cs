using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BankOrderCurrencySource : IEntity, IHasSaveLog
  {
    protected internal BankOrderCurrencySource()
    {
    }
    public int Id { get; set; }
    public bool HasFinancialDocumentBankOrder { get; set; }
    public int BankOrderId { get; set; }
    public double FOB { get; set; } // ارزش منشاء ارز
    public double TransferCost { get; set; } //  هزینه حمل
    public int BoxCount { get; set; } //  تعداد بسته
    public string SataCode { get; set; }
    public double ActualWeight { get; set; }
    public int UserId { get; set; }
    public int LadingId { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual BankOrder BankOrder { get; set; }
    public virtual Lading Lading { get; set; }
  }
}