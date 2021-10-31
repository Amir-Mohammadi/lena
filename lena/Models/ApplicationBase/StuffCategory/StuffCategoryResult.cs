using lena.Domains.Enums;
namespace lena.Models.StuffCategory
{
  public class StuffCategoryResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int? ParentStuffCategoryId { get; set; }
    public string ParentStuffCategoryName { get; set; }
    public int DefaultWarehouseId { get; set; }
    public string DefaultWarehouseName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}