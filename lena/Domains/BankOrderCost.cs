using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BankOrderCost : IEntity
  {
    protected internal BankOrderCost()
    {
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int FinancialDocumentCostId { get; set; }
    public double Amount { get; set; }
    public int BankOrderId { get; set; }
    public virtual FinancialDocumentCost FinancialDocumentCost { get; set; }
    public virtual BankOrder BankOrder { get; set; }
  }
}