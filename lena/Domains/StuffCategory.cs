using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffCategory : IEntity
  {
    protected internal StuffCategory()
    {
      this.Stuffs = new HashSet<Stuff>();
      this.SubStuffCategories = new HashSet<StuffCategory>();
    }
    public short Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public Nullable<short> ParentStuffCategoryId { get; set; }
    public byte[] RowVersion { get; set; }
    public short DefaultWarehouseId { get; set; }
    public virtual ICollection<Stuff> Stuffs { get; set; }
    public virtual ICollection<StuffCategory> SubStuffCategories { get; set; }
    public virtual StuffCategory ParentStuffCategory { get; set; }
    public virtual Warehouse DefaultWarehouse { get; set; }
  }
}