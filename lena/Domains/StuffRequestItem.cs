using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffRequestItem : BaseEntity, IEntity
  {
    protected internal StuffRequestItem()
    {
      this.ResponseStuffRequestItems = new HashSet<ResponseStuffRequestItem>();
    }
    public Nullable<int> StuffId { get; set; }
    public double Qty { get; set; }
    public double ResponsedQty { get; set; }
    public byte UnitId { get; set; }
    public int StuffRequestId { get; set; }
    public StuffRequestItemStatusType Status { get; set; }
    public Nullable<short> BillOfMaterialVersion { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual StuffRequest StuffRequest { get; set; }
    public virtual ICollection<ResponseStuffRequestItem> ResponseStuffRequestItems { get; set; }
    public virtual BillOfMaterial BillOfMaterial { get; set; }
  }
}