using lena.Domains.Enums;
namespace lena.Models
{
  public class AddBankOrderCurrencySourceInput
  {
    public int BankOrderId { get; set; }
    public int LadingId { get; set; }
    public double TransferCost { get; set; }
    public int BoxCount { get; set; }
    public double FOB { get; set; }
    public string SataCode { get; set; }
    public double ActualWeight { get; set; }

  }
}