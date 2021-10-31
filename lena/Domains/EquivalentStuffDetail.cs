using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EquivalentStuffDetail : IEntity
  {
    protected internal EquivalentStuffDetail()
    {
    }
    public int Id { get; set; }
    public double Value { get; set; }
    public byte UnitId { get; set; }
    public double ForQty { get; set; }
    public int StuffId { get; set; }
    public Nullable<short> SemiProductBillOfMaterialVersion { get; set; }
    public int EquivalentStuffId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual BillOfMaterial SemiProductBillOfMaterial { get; set; }
    public virtual EquivalentStuff EquivalentStuff { get; set; }
  }
}
