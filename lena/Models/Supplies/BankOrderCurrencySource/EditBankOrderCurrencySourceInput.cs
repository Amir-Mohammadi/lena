using lena.Domains.Enums;
namespace lena.Models
{
  public class EditBankOrderCurrencySourceInput
  {
    public int Id { get; set; }
    public int LadingId { get; set; }
    public double TransferCost { get; set; }
    public int BoxCount { get; set; }
    public double FOB { get; set; }
    public string SataCode { get; set; }
    public double ActualWeight { get; set; }
    public byte[] RowVersion { get; set; }

  }
}