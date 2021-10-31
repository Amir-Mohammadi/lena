using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CostCenter : IEntity
  {
    protected internal CostCenter()
    {
      this.PurchaseRequests = new HashSet<PurchaseRequest>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public CostCenterStatus Status { get; set; }
    public DateTime? ConfirmDateTime { get; set; }
    public int? ConfirmerUserId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User ConfirmerUser { get; set; }
    public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; }
  }
}
