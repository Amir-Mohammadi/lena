using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class DeleteCargoItemInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
