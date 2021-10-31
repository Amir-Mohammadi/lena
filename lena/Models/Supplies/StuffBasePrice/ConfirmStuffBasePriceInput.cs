using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffBasePrice
{
  public class ConfirmStuffBasePriceInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
  }
}