using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffRequest : BaseEntity, IEntity
  {
    protected internal StuffRequest()
    {
      this.StuffRequestItems = new HashSet<StuffRequestItem>();
    }
    public StuffRequestType StuffRequestType { get; set; }
    public Nullable<int> ScrumEntityId { get; set; }
    public short FromWarehouseId { get; set; }
    public Nullable<short> ToWarehouseId { get; set; }
    public Nullable<int> ProductionMaterialRequestId { get; set; }
    public Nullable<int> ToEmployeeId { get; set; }
    public Nullable<short> ToDepartmentId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<StuffRequestItem> StuffRequestItems { get; set; }
    public virtual ScrumEntity ScrumEntity { get; set; }
    public virtual Warehouse FromWarehouse { get; set; }
    public virtual Warehouse ToWarehouse { get; set; }
    public virtual ProductionMaterialRequest ProductionMaterialRequest { get; set; }
    public virtual Employee ToEmployee { get; set; }
    public virtual Department ToDepartment { get; set; }
  }
}