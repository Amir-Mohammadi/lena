using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ResponseStuffRequestItem : BaseEntity, IEntity
  {
    protected internal ResponseStuffRequestItem()
    {
    }
    public int StuffRequestItemId { get; set; }
    public double Qty { get; set; }
    public StuffRequestItemStatusType Status { get; set; }
    public Nullable<int> RequestWarehouseIssueId { get; set; }
    public Nullable<int> StuffId { get; set; }
    public Nullable<short> BillOfMaterialVersion { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StuffRequestItem StuffRequestItem { get; set; }
    public virtual RequestWarehouseIssue RequestWarehouseIssue { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual BillOfMaterial BillOfMaterial { get; set; }
  }
}