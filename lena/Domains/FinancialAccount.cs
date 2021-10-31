using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinancialAccount : IEntity
  {
    protected internal FinancialAccount()
    {
      this.FinancialTransactions = new HashSet<FinancialTransaction>();
      this.FinancialDocuments = new HashSet<FinancialDocument>();
      this.FinancialDocumentTransfers = new HashSet<FinancialDocumentTransfer>();
      this.FinancialAccountDetails = new HashSet<FinancialAccountDetail>();
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public byte CurrencyId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<FinancialTransaction> FinancialTransactions { get; set; }
    public virtual ICollection<FinancialDocument> FinancialDocuments { get; set; }
    public virtual ICollection<FinancialDocumentTransfer> FinancialDocumentTransfers { get; set; }
    public virtual ICollection<FinancialAccountDetail> FinancialAccountDetails { get; set; }
    public virtual Currency Currency { get; set; }
  }
}
