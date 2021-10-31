using lena.Domains.Enums;
namespace lena.Models.Supplies.SuppliesPurchaserUser
{
  public class UnDefaultSuppliesPurchaserUserInput
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public int PurchaserUserId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
