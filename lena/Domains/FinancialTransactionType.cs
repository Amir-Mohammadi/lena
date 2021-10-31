using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinancialTransactionType : IEntity
  {
    protected internal FinancialTransactionType()
    {
      this.FinancialTransactions = new HashSet<FinancialTransaction>();
      this.ReferenceFinancialTransactionTypes = new HashSet<FinancialTransactionType>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public FinancialTransactionLevel FinancialTransactionLevel { get; set; }
    public TransactionTypeFactor Factor { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<int> RollbackFinancialTransactionTypeId { get; set; }
    public virtual ICollection<FinancialTransaction> FinancialTransactions { get; set; }
    public virtual ICollection<FinancialTransactionType> ReferenceFinancialTransactionTypes { get; set; }
    public virtual FinancialTransactionType RollbackFinancialTransactionType { get; set; }
  }
}
