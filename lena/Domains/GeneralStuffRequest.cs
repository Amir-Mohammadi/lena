using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class GeneralStuffRequest : IEntity
  {
    protected internal GeneralStuffRequest()
    {
      this.GeneralStuffRequestDetails = new HashSet<GeneralStuffRequestDetail>();
    }
    public int Id { get; set; }
    public int? ProductionMaterialRequestId { get; set; }
    public virtual ProductionMaterialRequest ProductionMaterialRequest { get; set; }
    public StuffRequestType StuffRequestType { get; set; }
    public int StuffId { get; set; }
    public virtual Stuff Stuff { get; set; }
    public byte UnitId { get; set; }
    public virtual Unit Unit { get; set; }
    public short? BillOfMaterialVersion { get; set; }
    public virtual BillOfMaterial BillOfMaterial { get; set; }
    public double Qty { get; set; }
    public double StuffRequestQty { get; set; }
    public double PurchaseRequestQty { get; set; }
    public double AlternativePurchaseRequestQty { get; set; }
    public string StatusDescription { get; set; }
    public GeneralStuffRequestStatus Status { get; set; }
    public int? ScrumEntityId { get; set; }
    public ScrumEntity ScrumEntity { get; set; }
    public short FromWarehouseId { get; set; }
    public Warehouse FromWarehosue { get; set; }
    public short? ToWarehouseId { get; set; }
    public virtual Warehouse ToWarehouse { get; set; }
    public short? ToDepartmentId { get; set; }
    public virtual Department ToDepartment { get; set; }
    public int? ToEmployeeId { get; set; }
    public DateTime Deadline { get; set; }
    public Employee ToEmployee { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public virtual string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<GeneralStuffRequestDetail> GeneralStuffRequestDetails { get; set; }
  }
}
