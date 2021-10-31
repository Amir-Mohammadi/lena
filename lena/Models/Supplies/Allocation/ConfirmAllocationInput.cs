using lena.Domains.Enums;
namespace lena.Models
{
  public class ConfirmAllocationInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}