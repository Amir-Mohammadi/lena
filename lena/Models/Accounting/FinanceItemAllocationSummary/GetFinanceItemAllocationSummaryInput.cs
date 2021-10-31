using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceItemAllocationSummary
{
  public class GetFinanceItemAllocationSummaryInput
  {
    public int? FinanceId { get; set; }
    public int? CurrencyId { get; set; }
  }
}
