using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class ProviderFinancialDocumentResult
  {
    public int? ProviderId { get; set; }
    public string ProviderCode { get; set; }
    public string ProviderName { get; set; }
    public string PlanCode { get; set; }
    public double? CostSum { get; set; }
    public int? CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
  }
}
