using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WarehouseIssue : BaseEntity, IEntity
  {
    protected internal WarehouseIssue()
    {
      this.WarehouseIssueItems = new HashSet<WarehouseIssueItem>();
    }
    public WarehouseIssueStatusType Status { get; set; }
    public short FromWarehouseId { get; set; }
    public Nullable<short> ToWarehouseId { get; set; }
    public Nullable<short> ToDepartmentId { get; set; }
    public Nullable<int> ToEmployeeId { get; set; }
    public byte[] RowVersion { get; set; }

    public virtual ResponseWarehouseIssue ResponseWarehouseIssue { get; set; }
    public virtual Warehouse FromWarehouse { get; set; }
    public virtual Warehouse ToWarehouse { get; set; }
    public virtual ICollection<WarehouseIssueItem> WarehouseIssueItems { get; set; }
    public virtual Department ToDepartment { get; set; }
    public virtual Employee ToEmployee { get; set; }
  }
}