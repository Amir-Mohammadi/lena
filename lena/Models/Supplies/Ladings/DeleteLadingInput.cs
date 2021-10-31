using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class DeleteLadingInput
  {
    public int Id { get; set; }

    public byte[] RowVersion { get; set; }
  }
}
