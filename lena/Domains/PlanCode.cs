using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PlanCode : IEntity
  {
    protected internal PlanCode()
    {
      this.PurchaseRequests = new HashSet<PurchaseRequest>();
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }
    public DateTime RegisterDateTime { get; set; }
    public int RegisterarUserId { get; set; }
    public virtual User RegisterarUser { get; set; }
    public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; }
  }
}