using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionStuffDetail : IEntity
  {
    public ProductionStuffDetail()
    {
    }
    public int Id { get; set; }
    public int ProductionId { get; set; }
    public Nullable<int> ProductionOperationId { get; set; }
    public BillOfMaterialDetailType BillOfMaterialDetailType { get; set; }
    public int StuffId { get; set; }
    public Nullable<long> StuffSerialCode { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public byte[] RowVersion { get; set; }
    public short WarehouseId { get; set; }
    public ProductionStuffDetailType Type { get; set; }
    public double DetachedQty { get; set; }
    public virtual Production Production { get; set; }
    public virtual ProductionOperation ProductionOperation { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual StuffSerial StuffSerial { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual Warehouse Warehouse { get; set; }
  }
}
