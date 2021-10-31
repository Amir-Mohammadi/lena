using lena.Domains.Enums;
namespace lena.Models.Accounting.Finance
{
  public class EditFinanceInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public int CooperatorId { get; set; }
    public int FinancialAccountDetailId { get; set; }
    public byte CurrencyId { get; set; }
    public int[] AddFinanceItemIds { get; set; }
    public int[] DeleteFinanceItemIds { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
