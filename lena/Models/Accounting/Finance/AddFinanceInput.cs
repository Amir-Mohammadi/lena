using lena.Domains.Enums;
namespace lena.Models.Accounting.Finance
{
  public class AddFinanceInput
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public int CooperatorId { get; set; }
    public byte CurrencyId { get; set; }
    public int[] AddFinanceItemIds { get; set; }
  }
}
