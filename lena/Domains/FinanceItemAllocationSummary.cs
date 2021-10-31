using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class FinanceItemAllocationSummary : IEntity
  {
    protected internal FinanceItemAllocationSummary()
    {
      this.TotalAllocatedAmount = 0;
      this.TotalTransferredAmount = 0;
    }
    public int FinanceId { get; set; }
    public int CooperatorId { get; set; }
    public double TotalRequestedAmout { get; set; }
    public double TotalAllocatedAmount { get; set; }
    public double TotalTransferredAmount { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Finance Finance { get; set; }
    public virtual Cooperator Cooperator { get; set; }
    public virtual ICollection<FinanceItem> FinanceItem { get; set; }
  }
}
