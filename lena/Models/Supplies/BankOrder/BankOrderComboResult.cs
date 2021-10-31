using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrder
{
  public class BankOrderComboResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string OrderNumber { get; set; }
    public string FolderCode { get; set; }
    public string BankName { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
  }
}
