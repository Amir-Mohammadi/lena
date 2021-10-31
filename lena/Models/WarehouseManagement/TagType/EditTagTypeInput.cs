using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.TagType
{
  public class EditTagTypeInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Name { get; set; }
  }
}
