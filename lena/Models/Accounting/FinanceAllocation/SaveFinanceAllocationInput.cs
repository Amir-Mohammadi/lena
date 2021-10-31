using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceAllocation
{
  public class SaveFinanceAllocationInput
  {
    public int FinanceId { get; set; }
    public int FinancialAccountDetailId { get; set; }
    public int[] DeleteIds { get; set; }
    public FinanceAllocationDetailInput[] AddFinanceAllocationDetailInput { get; set; }
    public FinanceAllocationDetailInput[] EditFinanceAllocationDetailInput { get; set; }
    public byte[] FinanceRowVersion { get; set; }
  }
}
