using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionMaterialRequestDetail : IEntity
  {
    protected internal ProductionMaterialRequestDetail()
    {
    }
    public int Id { get; set; }
    public int ProductionOrderId { get; set; }
    public virtual ProductionOrder ProductionOrder { get; set; }
    public int ProductionMaterialRequestId { get; set; }
    public virtual ProductionMaterialRequest ProductionMaterialRequest { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
