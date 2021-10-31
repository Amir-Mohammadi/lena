using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequestItem
{
  public class DeleteStuffRequestItemInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
