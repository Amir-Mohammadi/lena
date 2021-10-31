using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinancialDocument : BaseEntity, IEntity
  {
    protected internal FinancialDocument()
    {
      this.FinanceItems = new HashSet<FinanceItem>();
      this.BankOrderIssues = new HashSet<BankOrderIssue>();
    }
    public FinancialDocumentType Type { get; set; }
    public int FinancialAccountId { get; set; }
    public Nullable<Guid> DocumentId { get; set; }
    public DateTime DocumentDateTime { get; set; }
    public double DebitAmount { get; set; }
    public double CreditAmount { get; set; }
    public Nullable<int> FinanceId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual FinancialAccount FinancialAccount { get; set; }
    public virtual FinancialDocumentTransfer FinancialDocumentTransfer { get; set; }
    public virtual FinancialDocumentCost FinancialDocumentCost { get; set; }
    public virtual FinancialDocumentCorrection FinancialDocumentCorrection { get; set; }
    public virtual FinancialDocumentBeginning FinancialDocumentBeginning { get; set; }
    public virtual FinancialDocumentDiscount FinancialDocumentDiscount { get; set; }
    public virtual FinancialDocumentBankOrder FinancialDocumentBankOrder { get; set; }
    public virtual ICollection<FinanceItem> FinanceItems { get; set; }
    public virtual Finance Finance { get; set; }
    public virtual ICollection<BankOrderIssue> BankOrderIssues { get; set; }
  }
}
