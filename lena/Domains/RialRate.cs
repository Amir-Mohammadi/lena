using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class RialRate : IEntity
  {
    protected internal RialRate()
    {
      this.ReferencedRialRates = new HashSet<RialRate>();
    }
    public int Id { get; set; }
    public double Rate { get; set; }
    public Nullable<int> ReferenceRialRateId { get; set; }
    public byte[] RowVersion { get; set; }
    public double Amount { get; set; }
    public int FinancialTransactionId { get; set; }
    public bool IsValid { get; set; }
    public bool IsUsed { get; set; }
    public virtual ICollection<RialRate> ReferencedRialRates { get; set; }
    public virtual RialRate ReferenceRialRate { get; set; }
    public virtual FinancialTransaction FinancialTransaction { get; set; }
  }
}