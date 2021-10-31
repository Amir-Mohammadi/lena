using lena.Domains.Enums;
namespace lena.Models
{
  public class DeleteWarehouseInput
  {
    public short Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
