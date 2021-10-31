using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffPrice
{
  public class ConfirmStuffPriceInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
  }
}