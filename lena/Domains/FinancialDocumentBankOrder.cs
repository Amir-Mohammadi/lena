using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinancialDocumentBankOrder : IEntity
  {
    protected internal FinancialDocumentBankOrder()
    {
    }
    public int Id { get; set; }
    public int BankOrderId { get; set; }
    public double BankOrderAmount { get; set; }
    public double FOB { get; set; }
    public double TransferCost { get; set; }
    public int FinancialDocumentId { get; internal set; }
    public byte[] RowVersion { get; set; }
    public virtual BankOrder BankOrder { get; set; }
    public virtual FinancialDocument FinancialDocument { get; set; }
  }
}
