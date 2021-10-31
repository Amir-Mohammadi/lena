using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionMaterialRequest : BaseEntity, IEntity
  {
    protected internal ProductionMaterialRequest()
    {
      this.StuffRequests = new HashSet<StuffRequest>();
      this.GeneralStuffRequests = new HashSet<GeneralStuffRequest>();
    }
    [Obsolete("ProductionOrderId is deprecated, please use ProductionMaterialRequestDetails instead.", false)]
    public int? ProductionOrderId { get; set; }
    public byte[] RowVersion { get; set; }
    [Obsolete("ProductionOrder is deprecated, please use ProductionMaterialRequestDetails instead.", false)]
    public virtual ProductionOrder ProductionOrder { get; set; }
    public virtual ICollection<StuffRequest> StuffRequests { get; set; }
    public virtual ICollection<GeneralStuffRequest> GeneralStuffRequests { get; set; }
    public virtual ICollection<ProductionMaterialRequestDetail> ProductionMaterialRequestDetails { get; set; }
  }
}