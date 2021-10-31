using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class DeleteCargoItemDetailInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
