using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinancialTransaction : IEntity
  {
    protected internal FinancialTransaction()
    {
      this.ReferencedFinancialTransactions = new HashSet<FinancialTransaction>();
      this.RialRates = new HashSet<RialRate>();
    }
    public int Id { get; set; }
    public int FinancialTransactionBatchId { get; set; }
    public int FinancialTransactionTypeId { get; set; }
    public int FinancialAccountId { get; set; }
    public DateTime EffectDateTime { get; set; }
    public double Amount { get; set; }
    public string Description { get; set; }
    public Nullable<int> ReferenceFinancialTransactionId { get; set; }
    public byte[] RowVersion { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool? IsPermanent { get; set; }
    public virtual FinancialTransactionType FinancialTransactionType { get; set; }
    public virtual FinancialAccount FinancialAccount { get; set; }
    public virtual FinancialTransactionBatch FinancialTransactionBatch { get; set; }
    public virtual ICollection<FinancialTransaction> ReferencedFinancialTransactions { get; set; }
    public virtual FinancialTransaction ReferenceFinancialTransaction { get; set; }
    public virtual ICollection<RialRate> RialRates { get; set; }
  }
}
