using lena.Domains.Enums;
namespace lena.Models.Accounting.CooperatorFinancialAccount
{
  public class CooperatorFinancialAccountResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int? CooperatorId { get; set; }
    public string CooperatorCode { get; set; }
    public string CooperatorName { get; set; }
    public int? CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
