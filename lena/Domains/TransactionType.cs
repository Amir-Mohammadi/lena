using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class TransactionType : IEntity
  {
    protected internal TransactionType()
    {
      this.BaseTransactions = new HashSet<BaseTransaction>();
      this.ReferenceTransactionTypes = new HashSet<TransactionType>();
    }
    public short Id { get; set; }
    public string Name { get; set; }
    public TransactionTypeFactor Factor { get; set; }
    public TransactionLevel TransactionLevel { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<short> RollbackTransactionTypeId { get; set; }
    public virtual ICollection<BaseTransaction> BaseTransactions { get; set; }
    public virtual ICollection<TransactionType> ReferenceTransactionTypes { get; set; }
    public virtual TransactionType RollbackTransactionType { get; set; }
  }
}