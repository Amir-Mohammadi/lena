using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PurchaseRequestStepDetail : IEntity
  {
    protected internal PurchaseRequestStepDetail()
    {
      this.PurchaseRequests = new HashSet<PurchaseRequest>();
    }
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime DateTime { get; set; }
    public Nullable<int> PurchaseRequestStepId { get; set; }
    public byte[] RowVersion { get; set; }
    public int PurchaseRequestId { get; set; }
    public Nullable<Guid> DocumentId { get; set; }
    public string Description { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; }
    public virtual PurchaseRequestStep PurchaseRequestStep { get; set; }
    public virtual PurchaseRequest PurchaseRequest { get; set; }
  }
}