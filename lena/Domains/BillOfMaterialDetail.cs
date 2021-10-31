using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BillOfMaterialDetail : IEntity
  {
    protected internal BillOfMaterialDetail()
    {
      this.OperationConsumingMaterials = new HashSet<OperationConsumingMaterial>();
      this.EquivalentStuffs = new HashSet<EquivalentStuff>();
    }
    public int Id { get; set; }
    public int Index { get; set; }
    public double Value { get; set; }
    public bool Reservable { get; set; }
    public byte UnitId { get; set; }
    public int StuffId { get; set; }
    public double ForQty { get; set; }
    public bool IsPackingMaterial { get; set; }
    public Nullable<short> SemiProductBillOfMaterialVersion { get; set; }
    public BillOfMaterialDetailType BillOfMaterialDetailType { get; set; }
    public byte[] RowVersion { get; set; }
    public short BillOfMaterialVersion { get; set; }
    public int BillOfMaterialStuffId { get; set; }
    public string Description { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual BillOfMaterial SemiProductBillOfMaterial { get; set; }
    public virtual ICollection<OperationConsumingMaterial> OperationConsumingMaterials { get; set; }
    public virtual ICollection<EquivalentStuff> EquivalentStuffs { get; set; }
    public virtual BillOfMaterial BillOfMaterial { get; set; }
  }
}