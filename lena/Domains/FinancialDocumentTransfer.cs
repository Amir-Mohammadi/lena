using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinancialDocumentTransfer : IEntity
  {
    protected internal FinancialDocumentTransfer()
    {
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int ToFinancialAccountId { get; set; }
    public double ToDebitAmount { get; set; }
    public int FinancialDocumentId { get; set; }
    public virtual FinancialAccount ToFinancialAccount { get; set; }
    public virtual FinancialDocument FinancialDocument { get; set; }
  }
}