using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceAllocationSummary
{
  public class FinanceAllocationSummaryResult
  {
    public int Id { get; set; }
    public int FinanceId { get; set; }
    public double TotalRequestedAmount { get; set; }
    public double TotalAllocatedAmount { get; set; }
    public double TotalTransferAmount { get; set; }
    public double TotalSeparatedTransferAmount { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
