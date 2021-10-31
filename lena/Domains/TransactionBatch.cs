using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class TransactionBatch : IEntity
  {
    protected internal TransactionBatch()
    {
      this.BaseTransactions = new HashSet<BaseTransaction>();
    }
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<BaseTransaction> BaseTransactions { get; set; }
    public virtual User User { get; set; }
    public virtual BaseEntity BaseEntity { get; set; }
  }
}