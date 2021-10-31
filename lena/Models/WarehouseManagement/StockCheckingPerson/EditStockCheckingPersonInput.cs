using lena.Domains.Enums;
namespace lena.Models.StockCheckingPerson
{
  public class EditStockCheckingPersonInput
  {
    public int StockCheckingId { get; set; }
    public int UserId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
