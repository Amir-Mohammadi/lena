using lena.Domains.Enums;
namespace lena.Models.StuffCategory
{
  public class EditStuffCategoryInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public short? ParentStuffCategoryId { get; set; }
    public byte[] RowVersion { get; set; }
    public short DefaultWarehouseId { get; set; }
  }
}