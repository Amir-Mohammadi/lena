using lena.Domains.Enums;
namespace lena.Models.Supplies.Cargo
{
  public class DeleteCargoInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
