using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Production : IEntity, IHasSaveLog, IHasDescription
  {
    protected internal Production()
    {
      this.ProductionStuffDetails = new HashSet<ProductionStuffDetail>();
      this.ProductionOperations = new HashSet<ProductionOperation>();
      this.RepairProductions = new HashSet<RepairProduction>();
    }
    public int Id { get; set; }
    public int ProductionOrderId { get; set; }
    public long StuffSerialCode { get; set; }
    public int StuffSerialStuffId { get; set; }
    public ProductionStatus Status { get; set; }
    public ProductionType Type { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionOrder ProductionOrder { get; set; }
    public virtual ICollection<ProductionStuffDetail> ProductionStuffDetails { get; set; }
    public virtual ICollection<ProductionOperation> ProductionOperations { get; set; }
    public virtual StuffSerial StuffSerial { get; set; }
    public virtual ICollection<RepairProduction> RepairProductions { get; set; }
  }
}
