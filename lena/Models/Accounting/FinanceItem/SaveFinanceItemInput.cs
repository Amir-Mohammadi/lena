using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceItem
{
  public class SaveFinanceItemInput
  {
    public FinanceType FinanceType { get; set; }
    public int[] DeletedFinanceItemIds { get; set; }
    public FinanceItemDetailInput[] AddFinanceItemDetails { get; set; }
    public FinanceItemDetailInput[] EditFinanceItemDetails { get; set; }
  }
}
