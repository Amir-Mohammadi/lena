using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class FinancialTransactionBatch : IEntity
  {
    protected internal FinancialTransactionBatch()
    {
      this.FinancialTransactions = new HashSet<FinancialTransaction>();
    }
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<FinancialTransaction> FinancialTransactions { get; set; }
    public virtual User User { get; set; }
    public virtual BaseEntity BaseEntity { get; set; }
  }
}
