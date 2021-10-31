using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceItemAllocationSummary
{
  public class FinanceItemAllocationSummaryResult
  {
    public int FinanceId { get; set; }
    public int CooperatorId { get; set; }
    public double TotalRequestedAmount { get; set; }
    public double TotalAllocatedAmount { get; set; }
    public double TotalTransferAmount { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
