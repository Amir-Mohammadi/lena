using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.StuffCategory
{
  public class StuffCategoryTreeResult
  {
    public StuffCategoryTreeResult()
    {
      this.ChildCategories = new List<StuffCategoryTreeResult>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int CategoryLevel { get; set; }
    public int DefaultWarehouseId { get; set; }
    public string DefaultWarehouseName { get; set; }
    public byte[] RowVersion { get; set; }
    public IList<StuffCategoryTreeResult> ChildCategories { get; set; }
  }
}
