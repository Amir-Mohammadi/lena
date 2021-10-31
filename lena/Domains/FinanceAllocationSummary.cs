using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class FinanceAllocationSummary : IEntity
  {
    protected internal FinanceAllocationSummary()
    {
      this.AllocatedAmount = 0;
      this.SeparatedTransferAmount = 0;
      this.TransferredAmount = 0;
    }
    public int Id { get; set; }
    public double RequestedAmount { get; set; }
    public double AllocatedAmount { get; set; }
    public double SeparatedTransferAmount { get; set; }
    public double TransferredAmount { get; set; }
    public int FinanceId { get; internal set; }
    public byte[] RowVersion { get; set; }
    public virtual Finance Finance { get; set; }
  }
}
